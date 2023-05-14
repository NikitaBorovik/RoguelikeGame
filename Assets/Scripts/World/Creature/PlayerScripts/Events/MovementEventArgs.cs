using System;
using UnityEngine;
namespace App.World.Creatures.PlayerScripts.Events
{
    public class MovementEventArgs : EventArgs
    {
        public float speed;
        public Vector2 direction;
    }
}
