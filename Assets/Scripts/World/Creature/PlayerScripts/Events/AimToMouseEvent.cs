using System;
using UnityEngine;

namespace App.World.Creatures.PlayerScripts.Events
{ 
    public class AimToMouseEvent : MonoBehaviour
    {
        public event Action<AimToMouseEvent, AimToMouseEventArgs> OnAimToMouse;

        public void CallAimEvent(float angle, float playerPos, float mousePos)
        {
            AimToMouseEventArgs args = new AimToMouseEventArgs() { angle = angle, playerPos = playerPos, mousePos = mousePos };
            OnAimToMouse?.Invoke(this, args);
        }
    }
}