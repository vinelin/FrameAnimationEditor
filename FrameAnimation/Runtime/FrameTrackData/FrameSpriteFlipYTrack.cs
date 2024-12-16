﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    public class FrameSpriteFlipYTrack : FrameTrackBase<FrameSpriteFlipYData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            FrameSpriteFlipYPlayer result = new FrameSpriteFlipYPlayer();
            result.Init(root, this);
            return result;
        }
    }
}