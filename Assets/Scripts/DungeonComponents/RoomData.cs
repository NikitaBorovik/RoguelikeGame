using App.World.Creatures.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    private string roomId;
    private string roomModelId;
    private RoomModel roomModel;
    private GameObject prefab;
    private RoomNodeType roomNodeType;
    private Vector2Int roomLowerBound;
    private Vector2Int roomUpperBound;
    private List<string> childrenRooms;
    private string parentId;
    private List<Door> doors;
    private bool isPlaced = false;
    private DrawnRoom drawnRoom;
    private List<BaseEnemy> enemies;
    private INotifyRoomChanged notifieble;

    public RoomData()
    {
        ChildrenRooms = new List<string>();
        Doors = new List<Door>();
        Enemies = new List<BaseEnemy>();
    }
    public RoomData(string roomId, string roomModelId, GameObject prefab, RoomNodeType roomNodeType, Vector2Int roomLowerBound, Vector2Int roomUpperBound,
        Vector2Int roomModelLowerBound, Vector2Int roomModelUpperBound, RoomModel roomModel, INotifyRoomChanged notifieble)
    {
        this.RoomId = roomId;
        this.RoomModelId = roomModelId;
        this.Prefab = prefab;
        this.RoomNodeType = roomNodeType;
        this.RoomLowerBound = roomLowerBound;
        this.RoomUpperBound = roomUpperBound;
        this.RoomModel = roomModel;
        this.Notifieble = notifieble;
    }

    public DrawnRoom DrawnRoom { get => drawnRoom; set => drawnRoom = value; }
    public Vector2Int RoomLowerBound { get => roomLowerBound; set => roomLowerBound = value; }
    public Vector2Int RoomUpperBound { get => roomUpperBound; set => roomUpperBound = value; }
    public string ParentId { get => parentId; set => parentId = value; }
    public List<Door> Doors { get => doors; set => doors = value; }
    public List<BaseEnemy> Enemies { get => enemies; set => enemies = value; }
    public string RoomId { get => roomId; set => roomId = value; }
    public string RoomModelId { get => roomModelId; set => roomModelId = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public RoomNodeType RoomNodeType { get => roomNodeType; set => roomNodeType = value; }
    public List<string> ChildrenRooms { get => childrenRooms; set => childrenRooms = value; }
    public bool IsPlaced { get => isPlaced; set => isPlaced = value; }
    public RoomModel RoomModel { get => roomModel; set => roomModel = value; }
    public INotifyRoomChanged Notifieble { get => notifieble; set => notifieble = value; }
}