using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/Dungeon/Room")]
public class RoomModel : ScriptableObject
{
    
    [HideInInspector]
    public string id;
    public GameObject prefab;
    [HideInInspector]
    public GameObject lastPrefab;
    public RoomNodeType roomType;
    public Vector2Int leftBottomPoint;
    public Vector2Int rightTopPoint;
    [NonReorderable]
    public List<Door> doors;
    public Vector2Int[] enemySpawns;
    public Vector2Int[] rewardSpawns;
    public Vector2Int playerSpawn;
    public Vector2Int teleporter;
    
#if UNITY_EDITOR
    public void OnValidate()
    {
        if(lastPrefab==prefab || id == null || id == "")
        {
            id = GUID.Generate().ToString();
            lastPrefab = prefab;
            EditorUtility.SetDirty(this);
        }
        if(doors == null || doors.Count == 0)
        {
            Debug.Log("Doors are not populated!");
        }
        
    }
#endif
}
