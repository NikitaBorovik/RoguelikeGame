using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailableNodeTypesForRoom", menuName = "Scriptable Objects/Dungeon Structure/ Available Node Types For Room")]
public class AvailableNodeTypesForRoom : ScriptableObject
{
    public List<NodeTypeForRoom> list;
}
