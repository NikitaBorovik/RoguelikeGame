using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType", menuName = "Scriptable Objects/Dungeon Structure/ Room Node Type")]
public class RoomNodeType : ScriptableObject
{
    public string typeName;

    public bool displayInCreator = true;

    public bool isCorridor;

    public bool isTopBottomCorridor;

    public bool isRightLeftCorridor;

    public bool isEntrance;

    public bool isBoss;

    public bool isNone;

    private void OnValidate()
    {
        if(string.IsNullOrEmpty(typeName))
        {
            Debug.Log(nameof(typeName) + " must contain a value in object " + this.name.ToString());
        }
    }
}
