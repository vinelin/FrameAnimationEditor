﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    public class FrameSpriteOrderTrack : FrameTrackBase<FrameSpriteOrderData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            FrameSpriteOrderPlayer result = new FrameSpriteOrderPlayer();
            result.Init(root, this);
            return result;
        }
    }
}