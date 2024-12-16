using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    [Serializable]
    public class FrameRotationData : FrameDataBase {
        //[SerializeField] private Quaternion localRotation = Quaternion.identity;
        [SerializeField] private Vector3 localRotation = Vector3.zero;

        //public Quaternion LocalRotation => localRotation;
        public Vector3 LocalRotation => localRotation;
    }
}