using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Creatures.PlayerScripts.Events
{ 
    public class AimEvent : MonoBehaviour
    {
        public event Action<AimEvent, AimEventArgs> OnAim;

        public void CallAimEvent(float angle, float playerPos, float mousePos)
        {
            AimEventArgs args = new AimEventArgs() { angle = angle, playerPos = playerPos, mousePos = mousePos };
            OnAim?.Invoke(this, args);
        }
    }
}