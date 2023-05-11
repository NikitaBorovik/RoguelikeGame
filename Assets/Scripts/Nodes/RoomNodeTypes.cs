using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeTypes", menuName = "Scriptable Objects/Dungeon Structure/ Room Node Types")]
public class RoomNodeTypes : ScriptableObject
{
    public List<RoomNodeType> list;
}
