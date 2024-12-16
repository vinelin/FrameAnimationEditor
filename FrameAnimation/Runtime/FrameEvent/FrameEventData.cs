﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VAnimation {
    [Serializable]
    public class FrameEventData : FrameDataBase {
        [SerializeField] private string eventKey;

        public string EventKey => eventKey;
    }
}