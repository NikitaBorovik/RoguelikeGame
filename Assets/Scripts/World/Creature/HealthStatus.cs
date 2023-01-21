using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HealthStatus : MonoBehaviour
{
    private int startingHealth;
    private int curHealth;
    

    public int StartingHealth {
        get
        { 
            return startingHealth; 
        }
        set 
        { 
            startingHealth = value;
            curHealth = startingHealth;
        }
      }
}
