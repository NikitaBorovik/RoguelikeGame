using System;
namespace App.World.Creatures.PlayerScripts.Events 
{
    public class AimEventArgs : EventArgs
    {
        public float angle;
        public float playerPos;
        public float mousePos;
    }
}


