using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    [Serializable]
    public class FrameVisibleData : FrameDataBase {
        [SerializeField] private bool visible = true;

        public bool Visible => visible;
    }
}