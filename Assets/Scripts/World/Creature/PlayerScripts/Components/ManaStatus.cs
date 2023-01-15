using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaStatus : MonoBehaviour
{
    private int startingMana;
    private int curMana;


    public int StartingMana
    {
        get
        {
            return startingMana;
        }
        set
        {
            startingMana = value;
            curMana = startingMana;
        }
    }
}
