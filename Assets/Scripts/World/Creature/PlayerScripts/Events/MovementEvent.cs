using System;
using UnityEngine;
namespace App.World.Creatures.PlayerScripts.Events
{
    public class MovementEvent : MonoBehaviour
    {
        public event Action<MovementEvent, MovementEventArgs> OnMove;

        public void CallMovementEvent(Vector2 direction, float speed)
        {
            MovementEventArgs args = new MovementEventArgs() { direction = direction, speed = speed };
            OnMove?.Invoke(this, args);
        }
    }
}
