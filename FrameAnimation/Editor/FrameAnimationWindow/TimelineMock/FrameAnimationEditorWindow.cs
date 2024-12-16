using UnityEditor;

namespace VAnimation.Editor{
    public abstract class FrameAnimationEditorWindow : EditorWindow{
        public abstract bool locked { get; set; }

        /// <summary>
        /// 允许编辑显示的Animation
        /// </summary>
        /// <param name="sequence">展示的资产</param>
        public abstract void SetAnimation(FrameAnimationAsset sequence);

        public abstract void SetAnimation(FrameAnimationCtrlBase director);
        public abstract void ClearAnimation();
    }
}