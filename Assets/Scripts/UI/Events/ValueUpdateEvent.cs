using App.UI.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.UI
{
    public class ValueUpdateEvent : MonoBehaviour
    {
        public event Action<ValueUpdateEvent, ValueUpdateEventArgs> OnValueUpdate;

        public void CallValueUpdateEvent(float prevValue, float newValue, float maxValue, string valueUpdateEventName)
        {
            ValueUpdateEventArgs args = new() { prevValue = prevValue, newValue = newValue, maxValue = maxValue , valueUpdateEventName = valueUpdateEventName };
            OnValueUpdate?.Invoke(this, args);
        }
    }

}
