using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeTypes", menuName = "Scriptable Objects/Dungeon Structure/ Room Node Types")]
public class RoomNodeTypes : ScriptableObject
{
    
    [Tooltip ("You should populate this list with all the RoomNodeType for the game.This list is used instead of enum")]
    public List<RoomNodeType> list;

    private void OnValidate()
    {
        int count = 0;
        foreach(var type in list)
        {

            if(type == null)
            {
                Debug.Log(nameof(list) + " contains null value in " + this.name.ToString());
            }
            else
            {
                count++;
            }

        }
        if (count == 0)
            Debug.Log(nameof(list) + " contains no value in " + this.name.ToString());
        
    }
}
