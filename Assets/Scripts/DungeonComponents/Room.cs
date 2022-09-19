using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public string roomId;
    public string roomModelId;
    public GameObject prefab;
    public RoomNodeType roomNodeType;
    public Vector2Int roomLowerBound;
    public Vector2Int roomUpperBound;
    public Vector2Int roomModelLowerBound;
    public Vector2Int roomModelUpperBound;
    public List<string> childrenRooms;
    public string parentId;
    public List<Door> doors;
    public bool isPlaced = false;
    public DrawnRoom drawnRoom;
    public Vector2Int[] enemySpawns;
    public Vector2Int[] rewardSpawns;
    public Vector2Int teleporter;
    public Vector2Int playerSpawn;
    public bool isCleared;
    public bool isVisible;
    public bool isPrev;

    public Room()
    {
        childrenRooms = new List<string>();
        doors = new List<Door>();
    }
    public Room(string roomId, string roomModelId, GameObject prefab, RoomNodeType roomNodeType, Vector2Int roomLowerBound, Vector2Int roomUpperBound,
        Vector2Int roomModelLowerBound, Vector2Int roomModelUpperBound, Vector2Int[] enemySpawns, Vector2Int[] rewardSpawns, Vector2Int teleporter, Vector2Int playerSpawn)
    {
        this.roomId = roomId;
        this.roomModelId = roomModelId;
        this.prefab = prefab;   
        this.roomNodeType = roomNodeType;
        this.roomLowerBound = roomLowerBound;
        this.roomUpperBound = roomUpperBound;
        this.roomModelLowerBound = roomModelLowerBound;
        this.roomModelUpperBound = roomModelUpperBound;
        this.enemySpawns = enemySpawns;
        this.rewardSpawns = rewardSpawns;
        this.teleporter = teleporter;
        this.playerSpawn = playerSpawn;

    }
}
