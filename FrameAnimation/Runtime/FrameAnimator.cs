using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MamiyaTool;

namespace VAnimation {
    [DisallowMultipleComponent]
    public class FrameAnimator : FrameAnimationCtrlBase {
        [CustomPropertyName("动画资源")] public List<FrameAnimationAsset> animations;
        [Space]
        [CustomPropertyName("动画名称")] public string animationName;
        [CustomPropertyName("动画序号")] public int index;
        [Space]
        [SerializeField, InspectorButton("PlayByName", "按名称播放")] private byte playByName;
        [SerializeField, InspectorButton("PlayByIndex", "按序号播放")] private byte playIndex;
        [SerializeField, InspectorButton("PlayNext", "播放下一个")] private byte playNext;
        [Space]
        [SerializeField, CustomPropertyName("当前帧数")] private int curFrame;
        [SerializeField, CustomPropertyName("当前时间")] private float curTime;
        [SerializeField, InspectorButton("PlayNextFrame", "下一帧")] private byte nextFrame;
        [SerializeField, InspectorButton("PlayPrevFrame", "上一帧")] private byte preFrame;
        [Space]
        [SerializeField, InspectorButton("Pause", "暂停")] private byte pause;
        [SerializeField, InspectorButton("Resume", "继续")] private byte resume;
        [SerializeField, InspectorButton("Stop", "停止")] private byte stop;
        [Space(40)]
        [SerializeField] private List<ParticalContent> particals = new List<ParticalContent>();

        private List<Coroutine> particalPlayCOs = new List<Coroutine>();
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        public override void Play() {
            PlayInner();
        }
        public override void Stop() {
            isPlaying = false;
            if(curAnim != null)
                curAnim.Reset();
            StopInner();
        }
        protected override bool Initialize() {
            if(animations != null && animations.Count > 0) {
                SetAnimation(animations[0]);
            }
            return true;
        }
        protected override void AwakeInner() {
            if(CanPlay && curAnim.PlayOnAwake)
                Play();
        }
        protected override void UpdateInner() {
            curFrame = curAnim == null ? 0 : curAnim.CurFrame;
            curTime = curAnim == null ? 0 : curAnim.CurTime;
        }
        protected override void PlayInner() {
            PlayAllPartical();
            base.PlayInner();
        }
        protected override void StopInner() {
            StopAllPartical();
            base.StopInner();
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public FrameAnimationAsset GetAnimation(string name) {
            foreach(var anim in animations) {
                if(anim.name == name)
                    return anim;
            }
            return null;
        }
        public void SetAnimation(string name) {
            var anim = GetAnimation(name);
            SetAnimation(anim);
        }
        public void Play(string name) {
            SetAnimation(name);
            Play();
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void PlayByName() {
            Play(animationName);
        }
        private void PlayNext() {
            var cur = curAnim == null ? GetAnimation(animationName) : curAnim.Asset;
            if(cur == null)
                return;
            int index = animations.IndexOf(cur);
            index++;
            if(index >= animations.Count)
                index = 0;
            Play(animations[index]);
        }
        private void PlayByIndex() {
            if(animations == null || animations.Count <= 0)
                return;
            if(index < 0 || index >= animations.Count)
                return;
            Play(animations[index]);
        }
        private void PlayNextFrame() {
            if(IsPlaying)
                Pause();
            if(curAnim == null) {
                Play(animationName);
                Pause();
            }
            if(curAnim == null)
                return;
            curAnim.NextFrame();
        }
        private void PlayPrevFrame() {
            if(IsPlaying)
                Pause();
            if(curAnim == null) {
                Play(animationName);
                Pause();
            }
            if(curAnim == null)
                return;
            curAnim.PrevFrame();
        }

        private void StopAllPartical() {
            foreach(var co in particalPlayCOs) {
                if(co != null)
                    StopCoroutine(co);
            }
            particalPlayCOs.Clear();
        }
        private void PlayAllPartical() {
            StopAllPartical();
            foreach(var pc in particals) {
                if(pc.partical != null) {
                    particalPlayCOs.Add(StartCoroutine(PlayParticalCO(pc.time, pc.partical)));
                }
            }
        }
        private IEnumerator PlayParticalCO(float time, ParticleSystem partical) {
            float timer = 0;
            while(timer < time) {
                yield return null;
                timer += Time.deltaTime;
            }
            partical.Play();
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        [Serializable]
        public struct ParticalContent {
            [CustomPropertyName("延迟时间")] public float time;
            [CustomPropertyName("粒子效果")] public ParticleSystem partical;
        }
    }
}