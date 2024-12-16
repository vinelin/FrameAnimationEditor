using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    public class FrameSpritePlayer : FramePlayerBase<SpriteRenderer, FrameSpriteData> {
        private Sprite cache;
        private MaterialPropertyBlock _mpb;

        private static int _UseNormalMap = Shader.PropertyToID("_UseNormalMap");
        private static int _BumpMap = Shader.PropertyToID("_BumpMap");

        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override void Cache() {
            cache = m_Object.sprite;
        }

        private void UpdateNormalFrame(FrameSpriteData data) {
            if (data.NormalFrame) {
                _mpb ??= new MaterialPropertyBlock();

                m_Object.GetPropertyBlock(_mpb);
                _mpb.SetFloat(_UseNormalMap, 1);
                _mpb.SetTexture(_BumpMap, data.NormalFrame);
                m_Object.SetPropertyBlock(_mpb);
            } else {
                if (_mpb != null) {
                    m_Object.GetPropertyBlock(_mpb);
                    _mpb.SetFloat(_UseNormalMap, 0);
                    //_mpb.SetTexture(_BumpMap, null);
                    m_Object.SetPropertyBlock(_mpb);
                }
            }
        }

        protected override void Invoke(FrameSpriteData data) {
            m_Object.sprite = data.Frame;
            UpdateNormalFrame(data);
        }
        protected override void ResetInner() {
            m_Object.sprite = cache;
        }

        protected override void OnDispose() {
            _mpb?.Clear();
            _mpb = null;
            base.OnDispose();
        }
    }
}