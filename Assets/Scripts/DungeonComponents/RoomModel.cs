using App.World.Creatures.Enemies;
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
    public RoomNodeType roomType;
    public Vector2Int leftBottomPoint;
    public Vector2Int rightTopPoint;
    [NonReorderable]
    public List<Door> doors;
    public Vector2Int[] enemySpawns;
    public Vector2Int playerSpawn;
    [SerializeField]
    public List<BaseEnemy> enemiesWave1;
    [SerializeField]
    public List<BaseEnemy> enemiesWave2;
    [SerializeField]
    public List<BaseEnemy> enemiesWave3;

}
