using App.World.Items.Staffs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongShootEvent : MonoBehaviour
{
    public event Action<StrongShootEvent> OnShoot;

    public void CallStrongShootEvent()
    {
        OnShoot?.Invoke(this);
    }
}
