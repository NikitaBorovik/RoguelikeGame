using System;
using UnityEngine;
namespace App.World.Creatures.PlayerScripts.Events
{
    public class MovementEventArgs : EventArgs
    {
        public Vector2 direction;
        public float speed;
    }
}
