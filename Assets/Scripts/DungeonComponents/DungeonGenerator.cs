using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[DisallowMultipleComponent]
public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private RoomNodeTypes roomNodeTypes;
    public Dictionary<string, RoomData> roomDictionary;
    private bool buildSuccessfull;
    private DungeonStructureGraph dungeonStructureGraph;
    private Dictionary<string, RoomModel> roomModelsDictionary;
    private List<RoomModel> roomModelsList;
    private INotifyRoomChanged notifyRoomChanged;

    public INotifyRoomChanged NotifyRoomChanged { get => notifyRoomChanged; set => notifyRoomChanged = value; }
    public RoomNodeTypes RoomNodeTypes { get => roomNodeTypes; set => roomNodeTypes = value; }

    private void Awake()
    {
        roomDictionary = new Dictionary<string, RoomData>();
        roomModelsDictionary = new Dictionary<string, RoomModel>();
        roomModelsList = new List<RoomModel>();
    }
    public void CreateDungeonForLevel(LevelModel curLevel)
    {
        buildSuccessfull = false;

        roomModelsList = curLevel.RoomModels;

        CreateRoomModelsDictionary();

        while (!buildSuccessfull)
        {
            if (curLevel.DungeonStructures.Count > 0)
            {
                Clear();
                dungeonStructureGraph = curLevel.DungeonStructures[Random.Range(0, curLevel.DungeonStructures.Count)];
                buildSuccessfull = TryToBuildDungeon(dungeonStructureGraph);
            }
            else
            {
                Debug.LogError("No dungeon structures in the level");
                break;
            }

        }
        if (buildSuccessfull)
            DrawDungeon();
        else
            Debug.LogError("Failed to build dungeon");

    }

    private void Clear()
    {
        foreach (var room in roomDictionary.Values.Where(room => room.DrawnRoom != null))
            Destroy(room.DrawnRoom.gameObject);
        
        roomDictionary.Clear();
    }

    private void DrawDungeon()
    {
        foreach (KeyValuePair<string, RoomData> pair in roomDictionary)
        {

            Vector3 position = new Vector3(pair.Value.RoomLowerBound.x - pair.Value.RoomModel.leftBottomPoint.x,
                pair.Value.RoomLowerBound.y - pair.Value.RoomModel.leftBottomPoint.y, 0f);
            DrawRoomOnCoords(pair.Value, position);
        }
    }

    private void DrawRoomOnCoords(RoomData room, Vector3 position)
    {
        GameObject roomGameObject = Instantiate(room.Prefab, position, Quaternion.identity, transform);
        DrawnRoom drawnRoom = roomGameObject.GetComponentInChildren<DrawnRoom>();
        drawnRoom.room = room;
        drawnRoom.DrawRoom(roomGameObject);
        room.DrawnRoom = drawnRoom;
    }

    private bool TryToBuildDungeon(DungeonStructureGraph dungeonStructureGraph)
    {
        bool isNotOverlapping = true;
        Queue<RoomNode> roomsToPlace = new Queue<RoomNode>();
        RoomNode entrance = dungeonStructureGraph.GetNode(RoomNodeTypes.list.Find(x => x.isEntrance));
        if (entrance == null)
        {
            return false;
        }
        roomsToPlace.Enqueue(entrance);
        isNotOverlapping = PlaceAllGraphRooms(dungeonStructureGraph, roomsToPlace);
        return roomsToPlace.Count == 0 && isNotOverlapping;
    }

    private bool PlaceAllGraphRooms(DungeonStructureGraph dungeonStructureGraph, Queue<RoomNode> roomsToPlace)
    {
        while (roomsToPlace.Count > 0)
        {
            bool noOverlap = true;
            RoomNode roomNode = roomsToPlace.Dequeue();
            List<RoomNode> roomChildren = dungeonStructureGraph.GetChildrenNodes(roomNode);
            for (int i = 0; i < roomChildren.Count; i++)
            {
                roomsToPlace.Enqueue(roomChildren[i]);
            }
            if (roomNode.roomType.isEntrance)
                PlaceEntrance(roomNode);
            else
            {
                noOverlap = PlaceRoomWithoutOverlaps(roomNode, roomDictionary[roomNode.parentId]);
            }
            if (!noOverlap)
                return false;
        }
        return true;
    }

    private RoomData PlaceCorridor(RoomData parent)
    {
        if (parent == null)
            return null;
        RoomData room = null;
        int tryCount = 0;
        while (room == null && tryCount < 4)
        {
            tryCount++;

            List<Door> AvailableDoorsOnParentRoom = FindAvailableDoor(parent.Doors);
            if (AvailableDoorsOnParentRoom.Count == 0)
            {
                continue;
            }
            RoomNode roomNode = ScriptableObject.CreateInstance<RoomNode>();
            roomNode.Initialise(dungeonStructureGraph, RoomNodeTypes.list.Find(x => x.isCorridor));
            roomNode.roomType.isCorridor = true;
            roomNode.roomType.isNone = false;
            Door doorOfParentToConnect = AvailableDoorsOnParentRoom[Random.Range(0, AvailableDoorsOnParentRoom.Count)];
            room = GenerateRoomUsingModel(GetRandomRoomModelWithAppropriateDoor(roomNode, doorOfParentToConnect), roomNode);
            if (ChechIfRoomCanBePlaced(parent, room, doorOfParentToConnect))
            {
                room.IsPlaced = true;
            }
            else
            {
                room = null;
            }
        }
        return room;

    }
    private bool PlaceRoomWithoutOverlaps(RoomNode roomNode, RoomData parent)
    {
        
        int tryCount = 0;
        bool isOverlaping = true;
        parent = PlaceCorridor(parent);
        while (isOverlaping && tryCount < 3)
        {

            if (parent == null)
            {
                tryCount++;
                continue;
            }
            List<Door> AvailableDoorsOnParentRoom = FindAvailableDoor(parent.Doors);
            tryCount++;


            if (AvailableDoorsOnParentRoom.Count != 0)
            {
                Door doorOfParentToConnect = AvailableDoorsOnParentRoom[Random.Range(0, AvailableDoorsOnParentRoom.Count)];
                RoomData roomToPlace = GenerateRoomUsingModel(GetRandomRoomModelWithAppropriateDoor(roomNode, doorOfParentToConnect), roomNode);
                if (!ChechIfRoomCanBePlaced(parent, roomToPlace, doorOfParentToConnect))
                    isOverlaping = true;
                else
                {
                    isOverlaping = false;
                    roomDictionary.Add(parent.RoomId, parent);
                    roomDictionary.Add(roomToPlace.RoomId, roomToPlace);
                    roomToPlace.IsPlaced = true;
                }

            }
            else
                return false;
        }
        return !isOverlaping;
    }

    private bool ChechIfRoomCanBePlaced(RoomData parent, RoomData roomToPlace, Door doorOfParentToConnect)
    {
        Door roomDoorToConnect = null;
        if (doorOfParentToConnect.Orientation == Door.DoorOrientation.left)
            roomDoorToConnect = roomToPlace.Doors.Find(x => x.Orientation == Door.DoorOrientation.right);
        if (doorOfParentToConnect.Orientation == Door.DoorOrientation.right)
            roomDoorToConnect = roomToPlace.Doors.Find(x => x.Orientation == Door.DoorOrientation.left);
        if (doorOfParentToConnect.Orientation == Door.DoorOrientation.top)
            roomDoorToConnect = roomToPlace.Doors.Find(x => x.Orientation == Door.DoorOrientation.bottom);
        if (doorOfParentToConnect.Orientation == Door.DoorOrientation.bottom)
            roomDoorToConnect = roomToPlace.Doors.Find(x => x.Orientation == Door.DoorOrientation.top);
        if (roomDoorToConnect == null)
        {
            doorOfParentToConnect.IsAvailable = false;
            return false;
        }
        Vector2Int addMove = findDoorShift(roomDoorToConnect);
        Vector2Int doorOfParentRealPosition = parent.RoomLowerBound - parent.RoomModel.leftBottomPoint + doorOfParentToConnect.Pos;
        roomToPlace.RoomLowerBound = roomToPlace.RoomModel.leftBottomPoint + doorOfParentRealPosition + addMove - roomDoorToConnect.Pos;
        roomToPlace.RoomUpperBound = roomToPlace.RoomLowerBound + roomToPlace.RoomModel.rightTopPoint - roomToPlace.RoomModel.leftBottomPoint;

        bool overlapFound = false;
        foreach (KeyValuePair<string, RoomData> pair in roomDictionary)
        {
            RoomData room = pair.Value;
            if (!room.IsPlaced || room.RoomId == roomToPlace.RoomId)
                continue;
            if (RoomsOverlapping(roomToPlace, room))
            {

                overlapFound = true;
                break;
            }

        }
        if (!overlapFound)
        {
            doorOfParentToConnect.IsConnected = true;
            doorOfParentToConnect.IsAvailable = false;
            roomDoorToConnect.IsConnected = true;
            roomDoorToConnect.IsAvailable = false;
            return true;
        }
        return false;
    }
    private Vector2Int findDoorShift(Door roomDoorToConnect)
    {
        if (roomDoorToConnect.Orientation == Door.DoorOrientation.left)
        {
            return new Vector2Int(1, 0);
        }
        if (roomDoorToConnect.Orientation == Door.DoorOrientation.right)
        {
            return new Vector2Int(-1, 0);
        }
        if (roomDoorToConnect.Orientation == Door.DoorOrientation.top)
        {
            return new Vector2Int(0, -1);
        }
        if (roomDoorToConnect.Orientation == Door.DoorOrientation.bottom)
        {
            return new Vector2Int(0, 1);
        }
        return Vector2Int.zero;
    }
    private bool RoomsOverlapping(RoomData firstRoom, RoomData secondRoom)
    {
        return CheckIntervalForOverlaps(firstRoom.RoomLowerBound.x, firstRoom.RoomUpperBound.x, secondRoom.RoomLowerBound.x, secondRoom.RoomUpperBound.x) 
                && CheckIntervalForOverlaps(firstRoom.RoomLowerBound.y, firstRoom.RoomUpperBound.y, secondRoom.RoomLowerBound.y, secondRoom.RoomUpperBound.y);
    }

    private bool CheckIntervalForOverlaps(int x11, int x12, int x21, int x22)
    {
        return Mathf.Max(x11, x21) <= Mathf.Min(x12, x22);
    }
    private RoomModel GetRandomRoomModelWithAppropriateDoor(RoomNode roomNode, Door doorOfParentToConnect)
    {
        int randNum;
        RoomModel roomModel = null;
        if (!roomNode.roomType.isCorridor)
        {
            roomModel = ChooseRandomModelForType(roomNode.roomType);
        }
        else
        {
            if (doorOfParentToConnect.Orientation == Door.DoorOrientation.top)
            {
                randNum = Random.Range(0, 3);
                switch (randNum)
                {
                    case 0:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isTopBottomCorridor));
                        break;
                    case 1:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isBottomLeftCorridor));
                        break;
                    case 2:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isBottomRightCorridor));
                        break;
                }
            }
            if (doorOfParentToConnect.Orientation == Door.DoorOrientation.bottom)
            {
                randNum = Random.Range(0, 3);
                switch (randNum)
                {
                    case 0:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isTopBottomCorridor));
                        break;
                    case 1:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isTopRightCorridor));
                        break;
                    case 2:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isTopLeftCorridor));
                        break;
                }
            }

            if (doorOfParentToConnect.Orientation == Door.DoorOrientation.left)
            {
                randNum = Random.Range(0, 3);
                switch (randNum)
                {
                    case 0:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isRightLeftCorridor));
                        break;
                    case 1:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isBottomRightCorridor));
                        break;
                    case 2:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isTopRightCorridor));
                        break;
                }
            }
            if (doorOfParentToConnect.Orientation == Door.DoorOrientation.right)
            {
                randNum = Random.Range(0, 3);
                switch (randNum)
                {
                    case 0:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isRightLeftCorridor));
                        break;
                    case 1:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isBottomLeftCorridor));
                        break;
                    case 2:
                        roomModel = ChooseRandomModelForType(RoomNodeTypes.list.Find(x => x.isTopLeftCorridor));
                        break;
                }
            }
        }
        return roomModel;
    }

    private List<Door> FindAvailableDoor(List<Door> doors)
    {
        List<Door> result = new List<Door>();
        foreach (Door door in doors)
        {
            if (door.IsAvailable && !door.IsConnected)
                result.Add(door);
        }
        return result;
    }

    private void PlaceEntrance(RoomNode roomNode)
    {
        RoomModel roomModel = ChooseRandomModelForType(roomNode.roomType);
        RoomData finalRoom = GenerateRoomUsingModel(roomModel, roomNode);
        finalRoom.IsPlaced = true;
        // List<Door> doors = finalRoom.Doors;
        roomDictionary.Add(finalRoom.RoomId, finalRoom);
    }

    private RoomData GenerateRoomUsingModel(RoomModel roomModel, RoomNode roomNode)
    {
        RoomData room = new RoomData(roomNode.id, roomModel.id, roomModel.prefab, roomModel.roomType, roomModel.leftBottomPoint, roomModel.rightTopPoint, roomModel.leftBottomPoint, roomModel.rightTopPoint, roomModel, NotifyRoomChanged);
        room.ChildrenRooms = roomNode.children;
        room.Doors = roomModel.doors.Select(door => new Door(door.Pos, door.Orientation, door.Prefab, door.WallToBuildPosition, door.WallBuildingWidthInTiles, door.WallBuildingHeigthInTiles)).ToList();
        if (roomNode.parentId == null)
        {
            room.ParentId = "";
        }
        else
            room.ParentId = roomNode.parentId;
        return room;
    }

    private RoomModel ChooseRandomModelForType(RoomNodeType roomType)
    {
        var roomModelsOfType = roomModelsList.Where(room => room.roomType == roomType).ToList();

        return roomModelsOfType.Count > 0 ? roomModelsOfType[Random.Range(0, roomModelsOfType.Count)] : null;
    }

    private void CreateRoomModelsDictionary()
    {
        roomModelsDictionary = roomModelsList.GroupBy(room => room.id).ToDictionary(g => g.Key, g => g.First());
    }
    
}