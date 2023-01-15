using App.World.Creatures.PlayerScripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashEvent : MonoBehaviour
{
    public event Action<DashEvent, DashEventArgs> OnDash;

    public void CallDashEvent(Vector2 direction,float dashDistance, float dashTime)
    {
        DashEventArgs args = new DashEventArgs() { direction = direction, dashDistance = dashDistance, dashTime = dashTime };
        OnDash?.Invoke(this, args);
    }
}
