using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    [Serializable]
    public class FrameScaleData : FrameDataBase {
        [SerializeField] private Vector3 localScale = Vector3.one;

        public Vector3 LocalScale => localScale;
    }
}