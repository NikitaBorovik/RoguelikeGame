using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEvent : MonoBehaviour
{
    public event Action<ShootEvent> OnShoot;

    public void CallShootEvent()
    {
        OnShoot?.Invoke(this);
    }
}