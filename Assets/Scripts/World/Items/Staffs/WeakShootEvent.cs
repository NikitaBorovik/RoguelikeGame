using System;
using UnityEngine;
namespace App.World.Items.Staffs
{
    public class WeakShootEvent : MonoBehaviour
    {
        public event Action<WeakShootEvent> OnShoot;

        public void CallWeakShootEvent()
        {
            OnShoot?.Invoke(this);
        }
    }
}