using UnityEngine;

namespace VAnimation.Editor{
	interface IRowGUI
	{
		FrameAnimationAsset asset { get; }
		Rect boundingRect { get; }
		bool locked { get; }
		bool showMarkers { get; }
		bool muted { get; }
		
		Rect ToWindowSpace(Rect treeViewRect);
	}
}