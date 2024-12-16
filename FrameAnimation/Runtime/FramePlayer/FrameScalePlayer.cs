using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VAnimation {
    public class FrameScalePlayer : FramePlayerBase<Transform, FrameScaleData> {
        private Vector3 cache;

        private AnimationCurve curveX;
        private AnimationCurve curveY;
        private AnimationCurve curveZ;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override void DoInitCurve(int totalBeginFrame, int totalEndFrame) {
            if(datas.Count <= 0)
                return;
            if(totalBeginFrame == totalEndFrame)
                return;
            List<int> keys = new List<int>();
            Dictionary<int, Keyframe> keysX = new Dictionary<int, Keyframe>();
            Dictionary<int, Keyframe> keysY = new Dictionary<int, Keyframe>();
            Dictionary<int, Keyframe> keysZ = new Dictionary<int, Keyframe>();
            // 开始帧
            var beginData = datas.Where((v) => v.FrameIndex == beginFrame).First();
            keys.Add(totalBeginFrame);
            keysX.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalScale.x));
            keysY.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalScale.y));
            keysZ.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalScale.z));
            // 结束帧
            var endData = datas.Where((v) => v.FrameIndex == endFrame).First();
            keys.Add(totalEndFrame);
            keysX.Add(totalEndFrame, new Keyframe(1f, endData.LocalScale.x));
            keysY.Add(totalEndFrame, new Keyframe(1f, endData.LocalScale.y));
            keysZ.Add(totalEndFrame, new Keyframe(1f, endData.LocalScale.z));
            // 关键帧
            foreach(var data in datas) {
                if(keysX.ContainsKey(data.FrameIndex))
                    continue;
                keys.Add(data.FrameIndex);
                float smooth = Mathf.InverseLerp(totalBeginFrame, totalEndFrame, data.FrameIndex);
                keysX.Add(data.FrameIndex, new Keyframe(smooth, data.LocalScale.x));
                keysY.Add(data.FrameIndex, new Keyframe(smooth, data.LocalScale.y));
                keysZ.Add(data.FrameIndex, new Keyframe(smooth, data.LocalScale.z));
            }
            // 创建曲线
            curveX = new AnimationCurve(keysX.Values.ToArray());
            curveY = new AnimationCurve(keysY.Values.ToArray());
            curveZ = new AnimationCurve(keysZ.Values.ToArray());
        }
        protected override void DoLerp(float smooth) {
            Vector3 localScale = m_Object.localScale;
            if(curveX != null)
                localScale.x = curveX.Evaluate(smooth);
            if(curveY != null)
                localScale.y = curveY.Evaluate(smooth);
            if(curveZ != null)
                localScale.z = curveZ.Evaluate(smooth);
            m_Object.localScale = localScale;
        }
        protected override void Cache() {
            cache = m_Object.localScale;
        }
        protected override void Invoke(FrameScaleData data) {
            m_Object.localScale = data.LocalScale;
        }
        protected override void ResetInner() {
            m_Object.localScale = cache;
        }
    }
}