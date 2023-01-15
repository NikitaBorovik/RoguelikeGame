using System;
using UnityEngine;
namespace App.World.Creatures.PlayerScripts.Events
{
    public class StandEvent : MonoBehaviour
    {
        public event Action<StandEvent> OnStand;

        public void CallStandEvent()
        {
            OnStand?.Invoke(this);
        }
    }
}
