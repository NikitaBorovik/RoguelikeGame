using System;
using UnityEngine;

namespace App.World.Creatures.PlayerScripts.Events
{
    public class DieEvent : MonoBehaviour
    {
        public event Action<DieEvent> OnDied;

        public void CallDieEvent()
        {
            OnDied?.Invoke(this);
        }
    }
}