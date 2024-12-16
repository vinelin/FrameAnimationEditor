using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VAnimation {
    public class FrameRotationPlayer : FramePlayerBase<Transform, FrameRotationData> {
        private Quaternion cache;

        private AnimationCurve curveX;
        private AnimationCurve curveY;
        private AnimationCurve curveZ;
        private AnimationCurve curveW;
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
            //Dictionary<int, Keyframe> keysW = new Dictionary<int, Keyframe>();
            //// 开始帧
            //var beginData = datas.Where((v) => v.FrameIndex == beginFrame).First();
            //keys.Add(totalBeginFrame);
            //keysX.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalRotation.x));
            //keysY.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalRotation.y));
            //keysZ.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalRotation.z));
            //keysW.Add(totalBeginFrame, new Keyframe(0f, beginData.LocalRotation.w));
            //// 结束帧
            //var endData = datas.Where((v) => v.FrameIndex == endFrame).First();
            //keys.Add(totalEndFrame);
            //keysX.Add(totalEndFrame, new Keyframe(1f, endData.LocalRotation.x));
            //keysY.Add(totalEndFrame, new Keyframe(1f, endData.LocalRotation.y));
            //keysZ.Add(totalEndFrame, new Keyframe(1f, endData.LocalRotation.z));
            //keysW.Add(totalEndFrame, new Keyframe(1f, endData.LocalRotation.w));
            //// 关键帧
            //foreach(var data in datas) {
            //    if(keysX.ContainsKey(data.FrameIndex))
            //        continue;
            //    keys.Add(data.FrameIndex);
            //    float smooth = Mathf.InverseLerp(totalBeginFrame, totalEndFrame, data.FrameIndex);
            //    keysX.Add(data.FrameIndex, new Keyframe(smooth, data.LocalRotation.x));
            //    keysY.Add(data.FrameIndex, new Keyframe(smooth, data.LocalRotation.y));
            //    keysZ.Add(data.FrameIndex, new Keyframe(smooth, data.LocalRotation.z));
            //    keysW.Add(data.FrameIndex, new Keyframe(smooth, data.LocalRotation.w));
            //}
            //// 创建曲线
            //curveX = new AnimationCurve(keysX.Values.ToArray());
            //curveY = new AnimationCurve(keysY.Values.ToArray());
            //curveZ = new AnimationCurve(keysZ.Values.ToArray());
            //curveW = new AnimationCurve(keysW.Values.ToArray());
        }
        protected override void DoLerp(float smooth) {
            //Quaternion localRotaion = m_Object.localRotation;
            //if(curveX != null)
            //    localRotaion.x = curveX.Evaluate(smooth);
            //if(curveY != null)
            //    localRotaion.y = curveY.Evaluate(smooth);
            //if(curveZ != null)
            //    localRotaion.z = curveZ.Evaluate(smooth);
            //if(curveW != null)
            //    localRotaion.w = curveW.Evaluate(smooth);
            //m_Object.localRotation = localRotaion;
        }
        protected override void Cache() {
            cache = m_Object.localRotation;
        }
        protected override void Invoke(FrameRotationData data) {
            //m_Object.localRotation = data.LocalRotation;
            var a = Quaternion.Euler(data.LocalRotation);
            m_Object.localRotation = Quaternion.Euler(data.LocalRotation);
        }
        protected override void ResetInner() {
            m_Object.localRotation = cache;
        }
    }
}