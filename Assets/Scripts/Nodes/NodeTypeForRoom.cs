using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeTypeForRoom", menuName = "Scriptable Objects/Dungeon Structure/ NodeTypeForRoom")]
public class NodeTypeForRoom : ScriptableObject
{
    public string typeName;

    public bool displayInCreator = true;

    public bool isCorridor;

    public bool isTopBottomCorridor;

    public bool isRightLeftCorridor;

    public bool isTopLeftCorridor;

    public bool isTopRightCorridor;

    public bool isBottomLeftCorridor;

    public bool isBottomRightCorridor;

    public bool isRightTopCorridor;

    public bool isRightBottomCorridor;

    public bool isLeftTopCorridor;

    public bool isLeftBottomCorridor;

    public bool isEntrance;

    public bool isBoss;

    public bool isTreasure;

    public bool isNone;

}
