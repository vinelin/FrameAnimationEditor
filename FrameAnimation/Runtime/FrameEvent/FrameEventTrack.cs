﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    public class FrameEventTrack : FrameTrackBase<FrameEventData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            FrameEventPlayer result = new FrameEventPlayer();
            result.Init(root, this);
            return result;
        }
    }
}