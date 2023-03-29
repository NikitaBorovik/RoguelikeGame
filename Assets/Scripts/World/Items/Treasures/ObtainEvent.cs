using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.World.Items.Treasures
{
    public class ObtainEvent : MonoBehaviour
    {
        public event Action<ObtainEvent> OnObtain;

        public void CallObtainEvent()
        {
            OnObtain?.Invoke(this);
        }
    }
}

