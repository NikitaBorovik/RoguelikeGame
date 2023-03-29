using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.UI.Events
{
    public class ValueUpdateEventArgs : EventArgs
    {
        public float prevValue;
        public float newValue;
        public float maxValue;
        public string valueUpdateEventName;
    }
}

