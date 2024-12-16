using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    public class FrameSpriteTrack : FrameTrackBase<FrameSpriteData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            FrameSpritePlayer result = new FrameSpritePlayer();
            result.Init(root, this);
            return result;
        }
    }
}