using App.World.Creatures.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    private string roomId;
    private string roomModelId;
    private GameObject prefab;
    private RoomNodeType roomNodeType;
    private Vector2Int roomLowerBound;
    private Vector2Int roomUpperBound;
    private Vector2Int roomModelLowerBound;
    private Vector2Int roomModelUpperBound;
    private List<string> childrenRooms;
    private string parentId;
    private List<Door> doors;
    private bool isPlaced = false;
    private DrawnRoom drawnRoom;
    private Vector2Int[] enemySpawns;
    private Vector2Int[] rewardSpawns;
    private Vector2Int teleporter;
    private Vector2Int playerSpawn;
    private bool isCleared;
    private bool isVisible;
    private bool isPrev;
    private List<BaseEnemy> enemies;

    public Room()
    {
        ChildrenRooms = new List<string>();
        Doors = new List<Door>();
        Enemies = new List<BaseEnemy>();
    }
    public Room(string roomId, string roomModelId, GameObject prefab, RoomNodeType roomNodeType, Vector2Int roomLowerBound, Vector2Int roomUpperBound,
        Vector2Int roomModelLowerBound, Vector2Int roomModelUpperBound, Vector2Int[] enemySpawns, Vector2Int[] rewardSpawns, Vector2Int teleporter, Vector2Int playerSpawn, List<BaseEnemy> enemies)
    {
        this.RoomId = roomId;
        this.RoomModelId = roomModelId;
        this.Prefab = prefab;   
        this.RoomNodeType = roomNodeType;
        this.RoomLowerBound = roomLowerBound;
        this.RoomUpperBound = roomUpperBound;
        this.RoomModelLowerBound = roomModelLowerBound;
        this.RoomModelUpperBound = roomModelUpperBound;
        this.EnemySpawns = enemySpawns;
        this.RewardSpawns = rewardSpawns;
        this.Teleporter = teleporter;
        this.PlayerSpawn = playerSpawn;
        this.Enemies = enemies;
    }

    public DrawnRoom DrawnRoom { get => drawnRoom; set => drawnRoom = value; }
    public Vector2Int RoomLowerBound { get => roomLowerBound; set => roomLowerBound = value; }
    public Vector2Int RoomUpperBound { get => roomUpperBound; set => roomUpperBound = value; }
    public Vector2Int RoomModelLowerBound { get => roomModelLowerBound; set => roomModelLowerBound = value; }
    public Vector2Int RoomModelUpperBound { get => roomModelUpperBound; set => roomModelUpperBound = value; }
    public string ParentId { get => parentId; set => parentId = value; }
    public List<Door> Doors { get => doors; set => doors = value; }
    public Vector2Int[] EnemySpawns { get => enemySpawns; set => enemySpawns = value; }
    public Vector2Int[] RewardSpawns { get => rewardSpawns; set => rewardSpawns = value; }
    public Vector2Int Teleporter { get => teleporter; set => teleporter = value; }
    public Vector2Int PlayerSpawn { get => playerSpawn; set => playerSpawn = value; }
    public List<BaseEnemy> Enemies { get => enemies; set => enemies = value; }
    public string RoomId { get => roomId; set => roomId = value; }
    public string RoomModelId { get => roomModelId; set => roomModelId = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public RoomNodeType RoomNodeType { get => roomNodeType; set => roomNodeType = value; }
    public List<string> ChildrenRooms { get => childrenRooms; set => childrenRooms = value; }
    public bool IsPlaced { get => isPlaced; set => isPlaced = value; }
    public bool IsPrev { get => isPrev; set => isPrev = value; }
}
