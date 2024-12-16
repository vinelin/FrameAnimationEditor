using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    [Serializable]
    public class FrameSpriteData : FrameDataBase {
        [SerializeField] private Sprite frame;
        [SerializeField] private Texture normalFrame;

        public Sprite Frame => frame;
        public Texture NormalFrame => normalFrame;
    }
}