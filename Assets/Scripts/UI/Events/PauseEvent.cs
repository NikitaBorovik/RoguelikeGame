using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.UI
{
    public class PauseEvent : MonoBehaviour
    {
        public event Action<PauseEvent, PauseEventArgs> OnPause;

        public void CallPauseEvent(bool isPaused)
        {
            PauseEventArgs args = new() { isPaused = isPaused };
            OnPause?.Invoke(this, args);
        }
    }
}

