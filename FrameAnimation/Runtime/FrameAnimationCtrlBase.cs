using System;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    public abstract class FrameAnimationCtrlBase : MonoBehaviour, IFrameAnimationCtrl {
        [SerializeField, Range(0f, 1f)] protected float playSpeed = 1f;
        public float PlaySpeed { get => playSpeed; set => playSpeed = value; }
        public FrameAnimation CurAnim => curAnim;
        protected FrameAnimation curAnim;

        public virtual bool IsPlaying => isPlaying;
        protected bool isPlaying;
        protected bool CanPlay {
            get {
                bool result = curAnim != null;
                if(!result && isPlaying)
                    isPlaying = false;
                return result;
            }
        }

        protected Dictionary<string, TypeValue> m_Params = new Dictionary<string, TypeValue>();
        protected bool init;
        /******************************************************************
         *
         *      lifecycle
         *
         ******************************************************************/
        protected void Awake() {
            isPlaying = false;
            init = Initialize();
            AwakeInner();
        }
        protected void OnEnable() {
            OnEnableInner();
        }
        protected void Start() {
            StartInner();
        }
        protected void Update() {
            UpdateInner();
            UpdateAnim();
        }
        protected void OnDisable() {
            OnDisableInner();
        }
        protected void OnDestroy() {
            OnDestroyInner();
            curAnim?.Dispose();
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public abstract void Play();
        public abstract void Stop();
        protected abstract bool Initialize();
        public bool GetBool(string param) {
            if(m_Params.ContainsKey(param))
                return m_Params[param].vBool;
            return default;
        }
        public float GetFloat(string param) {
            if(m_Params.ContainsKey(param))
                return m_Params[param].vFloat;
            return default;
        }
        public int GetInt(string param) {
            if(m_Params.ContainsKey(param))
                return m_Params[param].vInt;
            return default;
        }
        public void SetBool(string param, bool value) {
            if(m_Params.ContainsKey(param))
                m_Params[param].vBool = value;
        }
        public void SetFloat(string param, float value) {
            if(m_Params.ContainsKey(param))
                m_Params[param].vFloat = value;
        }
        public void SetInt(string param, int value) {
            if(m_Params.ContainsKey(param))
                m_Params[param].vInt = value;
        }
        public void SetTrigger(string param) {
            if(m_Params.ContainsKey(param))
                m_Params[param].vAct?.Invoke();
        }
        public void SetAnimation(FrameAnimationAsset asset) {
            if(asset == null)
                return;
            if(curAnim != null && curAnim.Asset == asset)
                return;
            if (curAnim != null) {
                curAnim.Reset();
                curAnim.Dispose();
            }
            curAnim = new FrameAnimation(transform, asset);
            if(isPlaying)
                curAnim.Play();
        }
        public void Play(FrameAnimationAsset asset) {
            SetAnimation(asset);
            PlayInner();
        }
        public virtual void Pause() {
            isPlaying = false;
        }
        public virtual void Resume() {
            if(CanPlay)
                isPlaying = true;
        }
        /******************************************************************
         *
         *      protected method
         *
         ******************************************************************/
        protected virtual void AddParam(string key, int value) {
            if(!m_Params.ContainsKey(key))
                m_Params.Add(key, new TypeValue());
            m_Params[key].vInt = value;
        }
        protected virtual void AddParam(string key, float value) {
            if(!m_Params.ContainsKey(key))
                m_Params.Add(key, new TypeValue());
            m_Params[key].vFloat = value;
        }
        protected virtual void AddParam(string key, bool value) {
            if(!m_Params.ContainsKey(key))
                m_Params.Add(key, new TypeValue());
            m_Params[key].vBool = value;
        }
        protected virtual void AwakeInner() { }
        protected virtual void OnEnableInner() { }
        protected virtual void StartInner() { }
        protected virtual void UpdateInner() { }
        protected virtual void OnDisableInner() { }
        protected virtual void OnDestroyInner() { }
        protected virtual void UpdateAnim() {
            if(!CanPlay)
                return;
            if(!IsPlaying)
                return;
            isPlaying = curAnim.Update(Time.deltaTime * PlaySpeed);
        }
        protected virtual void PlayInner() {
            if(isPlaying)
                return;
            if(!CanPlay)
                return;
            isPlaying = true;
            curAnim.Play();
        }
        protected virtual void StopInner() {
            curAnim = null;
        }
        /******************************************************************
         *
         *      define
         *
         ******************************************************************/
        protected class TypeValue {
            public int vInt;
            public float vFloat;
            public bool vBool;
            public Action vAct;
            public void SetValue(int v) {
                vInt = v;
            }
            public void SetValue(float v) {
                vFloat = v;
            }
            public void SetValue(bool v) {
                vBool = v;
            }
            public void SetValue(Action v) {
                vAct = v;
            }
        }
    }
}