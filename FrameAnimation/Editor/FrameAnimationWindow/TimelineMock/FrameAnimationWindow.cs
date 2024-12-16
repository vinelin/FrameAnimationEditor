using System;
using UnityEditor;
using UnityEngine;

namespace VAnimation.Editor{
    [EditorWindowTitle(title = "FrameAnimation", icon = "UnityEditor.Timeline.TimelineWindow")]
    partial class FrameAnimationWindow : FrameAnimationEditorWindow{
        public Rect clientArea{ get; set; }
        public bool isDragging { get; set; }
        
        public override bool locked{ set; get; }

        private void OnEnable(){
            //最大化取消会执行OnEnable
            if (instance == null)
                instance = this;
        }

        public static FrameAnimationWindow instance { get; private set; }
        
        public override void SetAnimation(FrameAnimationAsset sequence){
            throw new System.NotImplementedException();
        }

        public override void SetAnimation(FrameAnimationCtrlBase director){
            throw new System.NotImplementedException();
        }

        public override void ClearAnimation(){
            throw new System.NotImplementedException();
        }
        
        
        [MenuItem("Tools/FrameAnimation", false, 1)]
        public static void ShowWindow(){
            GetWindow<FrameAnimationWindow>(typeof(SceneView));
            instance.Focus();
        }

        public override string ToString(){
            return "UnityEditor.Timeline.TimelineWindow";
        }
    }
}