using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VAnimation {
    public class FramePositionPlayer : FramePlayerBase<Transform, FramePositionData> {
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
            //if(datas.Count <= 0)
            //    return;
            //if(totalBeginFrame == totalEndFrame)
            //    return;
            //List<int> keys = new List<int>();
            //Dictionary<int, Keyframe> keysX = new Dictionary<int, Keyframe>();
            //Dictionary<int, Keyframe> keysY = new Dictionary<int, Keyframe>();
            //Dictionary<int, Keyframe> keysZ = new Dictionary<int, Keyframe>();
            //// 开始帧
            //var beginData = datas.Where((v) => v.FrameIndex == beginFrame).First();
            //keys.Add(totalBeginFrame);
            //keysX.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalPosition.x));
            //keysY.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalPosition.y));
            //keysZ.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalPosition.z));
            //// 结束帧
            //var endData = datas.Where((v) => v.FrameIndex == endFrame).First();
            //keys.Add(totalEndFrame);
            //keysX.Add(totalEndFrame, new Keyframe(1f, endData.LocalPosition.x));
            //keysY.Add(totalEndFrame, new Keyframe(1f, endData.LocalPosition.y));
            //keysZ.Add(totalEndFrame, new Keyframe(1f, endData.LocalPosition.z));
            //// 关键帧
            //foreach(var data in datas) {
            //    if(keysX.ContainsKey(data.FrameIndex))
            //        continue;
            //    keys.Add(data.FrameIndex);
            //    float smooth = Mathf.InverseLerp(totalBeginFrame, totalEndFrame, data.FrameIndex);
            //    keysX.Add(data.FrameIndex, new Keyframe(smooth, data.LocalPosition.x));
            //    keysY.Add(data.FrameIndex, new Keyframe(smooth, data.LocalPosition.y));
            //    keysZ.Add(data.FrameIndex, new Keyframe(smooth, data.LocalPosition.z));
            //}
            //// 创建曲线
            //curveX = new AnimationCurve(keysX.Values.ToArray());
            //curveY = new AnimationCurve(keysY.Values.ToArray());
            //curveZ = new AnimationCurve(keysZ.Values.ToArray());
        }
        protected override void DoLerp(float smooth) {
            //Vector3 localPos = m_Object.localPosition;
            //if(curveX != null)
            //    localPos.x = curveX.Evaluate(smooth);
            //if(curveY != null)
            //    localPos.y = curveY.Evaluate(smooth);
            //if(curveZ != null)
            //    localPos.z = curveZ.Evaluate(smooth);
            //m_Object.localPosition = localPos;
        }
        protected override void Cache() {
            cache = m_Object.localPosition;
        }
        protected override void Invoke(FramePositionData data) {
            m_Object.localPosition = data.LocalPosition;
        }
        protected override void ResetInner() {
            m_Object.localPosition = cache;
        }
    }
}