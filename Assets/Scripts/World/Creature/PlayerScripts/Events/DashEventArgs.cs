using System;
using UnityEngine;

namespace App.World.Creatures.PlayerScripts.Events
{
    public class DashEventArgs : EventArgs
    {
        public Vector2 direction;
        public float dashDistance;
        public float dashTime;
    }
}
