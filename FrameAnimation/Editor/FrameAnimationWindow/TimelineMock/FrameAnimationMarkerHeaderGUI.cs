using UnityEngine;

namespace VAnimation.Editor{
    public class FrameAnimationMarkerHeaderGUI : IRowGUI{
        public FrameAnimationAsset asset{ get; }
        public Rect boundingRect{ get; }
        public bool locked{ get; }
        public bool showMarkers{ get; }
        public bool muted{ get; }
        public Rect ToWindowSpace(Rect treeViewRect){
            throw new System.NotImplementedException();
        }
    }
}