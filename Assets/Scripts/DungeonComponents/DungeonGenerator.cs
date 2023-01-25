using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class DungeonGenerator : MonoBehaviour
{
    public Dictionary<string,Room> roomDictionary = new Dictionary<string,Room>();
    private Dictionary<string, RoomModel> roomModelsDictionary = new Dictionary<string, RoomModel>();
    private List<RoomModel> roomModelsList = new List<RoomModel>();
    private RoomNodeTypes roomNodeTypes;
    private bool buildSuccessfull;
    private DungeonStructureGraph dungeonStructureGraph;

    private void Awake()
    {
        roomNodeTypes = MyResources.Instance.roomNodeTypes;
        MyResources.Instance.myMaterial.SetFloat("_AlphaValue", 1f);
    }

    public bool GenerateDungeon(LevelModel curLevel)
    {
        buildSuccessfull = false;
        roomModelsList = curLevel.roomModels;
        CreateRoomModelsDictionary();
        int replaceAttempts = 0;
        while(!buildSuccessfull && replaceAttempts < Settings.maxReplaceAttempts)
        {
            if (curLevel.dungeonStructures.Count > 0)
            {
                Clear();
                dungeonStructureGraph = curLevel.dungeonStructures[Random.Range(0, curLevel.dungeonStructures.Count)];
                buildSuccessfull = TryToBuildDungeon(dungeonStructureGraph);
                replaceAttempts++;
            }
              
        }
        if (buildSuccessfull)
        {
            DrawDungeon();
        }
        return buildSuccessfull;
    }

    private void Clear()
    {
        if(roomDictionary.Count > 0)
        {
            foreach(KeyValuePair<string,Room> pair in roomDictionary)
            {
                Room room = pair.Value;
                if(room.DrawnRoom!=null)
                    Destroy(room.DrawnRoom.gameObject);
            }
            roomDictionary.Clear();
        }
    }

    private void DrawDungeon()
    {
        foreach(KeyValuePair<string,Room> pair in roomDictionary)
        {
            Room room = pair.Value;
            Vector3 positionOfTheRoom = new Vector3(room.RoomLowerBound.x - room.RoomModelLowerBound.x,
                room.RoomLowerBound.y - room.RoomModelLowerBound.y, 0f);
            GameObject roomGameObject = Instantiate(room.Prefab,positionOfTheRoom,Quaternion.identity,transform);
            DrawnRoom drawnRoom = roomGameObject.GetComponentInChildren<DrawnRoom>();
            drawnRoom.room = room;
            drawnRoom.DrawRoom(roomGameObject);
            room.DrawnRoom = drawnRoom;
        }
    }

    private bool TryToBuildDungeon(DungeonStructureGraph dungeonStructureGraph)
    {
        Queue<RoomNode> roomsToPlace = new Queue<RoomNode>();
        RoomNode entrance = dungeonStructureGraph.GetNode(roomNodeTypes.list.Find(x => x.isEntrance));
        if (entrance == null)
        {
    
            return false;
        }
        roomsToPlace.Enqueue(entrance);
        bool isNotOverlapping = true;
        isNotOverlapping = ProcessGraphRooms(dungeonStructureGraph, roomsToPlace);
        if (roomsToPlace.Count == 0 && isNotOverlapping)
            return true;
        return false;
    }

    private bool ProcessGraphRooms(DungeonStructureGraph dungeonStructureGraph, Queue<RoomNode> roomsToPlace)
    {
        while(roomsToPlace.Count > 0)
        {
            bool noOverlap = true;
            RoomNode roomNode = roomsToPlace.Dequeue();
            List<RoomNode> roomChildren = dungeonStructureGraph.GetChildrenNodes(roomNode);
            for(int i = 0; i < roomChildren.Count;i++)
            {
                roomsToPlace.Enqueue(roomChildren[i]);
            }
            if (roomNode.roomType.isEntrance)
                PlaceEntrance(roomNode);
            else
            {
                Room parent = roomDictionary[roomNode.parentId];
                noOverlap = PlaceRoomWithoutOverlaps(roomNode, parent);
            }
            if (!noOverlap)
                return false;
        }
        return true;
    }

    private Room PlaceCorridor(Room parent)
    {
        if (parent == null)
            return null;
        Room room = null;
        int tryCount = 0;
        while (room == null && tryCount < 1)
        {
            tryCount++;
            
            List<Door> AvailableDoorsOnParentRoom = FindAvailableDoor(parent.Doors);
            if (AvailableDoorsOnParentRoom.Count == 0)
            {
                continue;
            }
            RoomNode roomNode = ScriptableObject.CreateInstance<RoomNode>();
            roomNode.Initialise(dungeonStructureGraph, roomNodeTypes.list.Find(x => x.isCorridor));
            roomNode.roomType.isCorridor = true;
            roomNode.roomType.isNone = false;
            Door doorOfParentToConnect = AvailableDoorsOnParentRoom[Random.Range(0, AvailableDoorsOnParentRoom.Count)];
            RoomModel roomModel = GetRandomRoomModelWithAppropriateDoor(roomNode, doorOfParentToConnect);
            room = GenerateRoomUsingModel(roomModel, roomNode);
            if (ChechIfRoomCanBePlaced(parent, room, doorOfParentToConnect))
            {
                room.IsPlaced = true;
               // roomDictionary.Add(room.roomId, room);
            }
            else
            {
                room = null;
               // return null;
            }
        }
        return room;
        
    }
    private bool PlaceRoomWithoutOverlaps(RoomNode roomNode, Room parent)
    {
        bool isOverlaping = true;
        int tryCount = 0;
        while (isOverlaping && tryCount < 1)
        {
            parent = PlaceCorridor(parent);
            tryCount++;
            if (parent == null)
                continue;
            
            List<Door> AvailableDoorsOnParentRoom = FindAvailableDoor(parent.Doors);
            if (AvailableDoorsOnParentRoom.Count == 0)
            {
                return false;
            }
            Door doorOfParentToConnect = AvailableDoorsOnParentRoom[Random.Range(0, AvailableDoorsOnParentRoom.Count)];
            RoomModel roomModel = GetRandomRoomModelWithAppropriateDoor(roomNode, doorOfParentToConnect);
            Room roomToPlace = GenerateRoomUsingModel(roomModel, roomNode);
            if (ChechIfRoomCanBePlaced(parent, roomToPlace, doorOfParentToConnect))
            {
                isOverlaping = false;
                roomToPlace.IsPlaced = true;
                roomDictionary.Add(parent.RoomId, parent);
                roomDictionary.Add(roomToPlace.RoomId, roomToPlace);
            }
            else
            {
                isOverlaping = true;
            }
        }
        return !isOverlaping;
    }

    private bool ChechIfRoomCanBePlaced(Room parent, Room roomToPlace, Door doorOfParentToConnect)
    {
        Door roomDoorToConnect = null;
        if (doorOfParentToConnect.orientation == DoorOrientation.left)
            roomDoorToConnect = roomToPlace.Doors.Find(x => x.orientation == DoorOrientation.right);
        if (doorOfParentToConnect.orientation == DoorOrientation.right)
            roomDoorToConnect = roomToPlace.Doors.Find(x => x.orientation == DoorOrientation.left);
        if (doorOfParentToConnect.orientation == DoorOrientation.top)
            roomDoorToConnect = roomToPlace.Doors.Find(x => x.orientation == DoorOrientation.bottom);
        if (doorOfParentToConnect.orientation == DoorOrientation.bottom)
            roomDoorToConnect = roomToPlace.Doors.Find(x => x.orientation == DoorOrientation.top);
        if (roomDoorToConnect == null)
        {
            doorOfParentToConnect.isAvailable = false;
            return false;
        }
        Vector2Int adj = Vector2Int.zero;
        if (roomDoorToConnect.orientation == DoorOrientation.left)
        {
            adj = new Vector2Int(1, 0);
        }
        if (roomDoorToConnect.orientation == DoorOrientation.right)
        {
            adj = new Vector2Int(-1, 0);
        }
        if (roomDoorToConnect.orientation == DoorOrientation.top)
        {
            adj = new Vector2Int(0, -1);
        }
        if (roomDoorToConnect.orientation == DoorOrientation.bottom)
        {
            adj = new Vector2Int(0, 1);
        }
        Vector2Int doorOfParentRealPosition = parent.RoomLowerBound - parent.RoomModelLowerBound + doorOfParentToConnect.pos;
        roomToPlace.RoomLowerBound = roomToPlace.RoomModelLowerBound + doorOfParentRealPosition + adj - roomDoorToConnect.pos;
        roomToPlace.RoomUpperBound = roomToPlace.RoomLowerBound + roomToPlace.RoomModelUpperBound - roomToPlace.RoomModelLowerBound;
        
        bool overlapFound = false;
        foreach(KeyValuePair<string,Room> pair in roomDictionary)
        {
            Room room = pair.Value;
            if(!room.IsPlaced || room.RoomId == roomToPlace.RoomId)
                continue;
            if (RoomsOverlapping(roomToPlace, room))
            {
             
                overlapFound = true;
                break;
            }
                
        }
        if(!overlapFound)
        {
            doorOfParentToConnect.isConnected = true;
            doorOfParentToConnect.isAvailable = false;
            roomDoorToConnect.isConnected = true;
            roomDoorToConnect.isAvailable = false;
            return true;
        }
       // doorOfParentToConnect.isAvailable = false;
        return false;
    }

    private bool RoomsOverlapping(Room firstRoom, Room secondRoom)
    {
        bool isOnXOverlapping = CheckIntervalForOverlaps(firstRoom.RoomLowerBound.x, firstRoom.RoomUpperBound.x, secondRoom.RoomLowerBound.x, secondRoom.RoomUpperBound.x);
        bool isOnYOverlapping = CheckIntervalForOverlaps(firstRoom.RoomLowerBound.y, firstRoom.RoomUpperBound.y, secondRoom.RoomLowerBound.y, secondRoom.RoomUpperBound.y);
        
        return isOnXOverlapping && isOnYOverlapping;
    }

    private bool CheckIntervalForOverlaps(int x11, int x12, int x21, int x22)
    {
        return Mathf.Max(x11, x21) <= Mathf.Min(x12, x22);
    }

    //TODO change method to support more corridor types
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
            if(doorOfParentToConnect.orientation == DoorOrientation.top)
            {
                randNum = Random.Range(0, 3);
                switch (randNum)
                {
                    case 0:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isTopBottomCorridor));
                        break;
                    case 1:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isBottomLeftCorridor));
                        break;
                    case 2:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isBottomRightCorridor));
                        break;
                }
                //roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isTopBottomCorridor));
            }
            if (doorOfParentToConnect.orientation == DoorOrientation.bottom)
            {
                randNum = Random.Range(0, 3);
                switch (randNum)
                {
                    case 0:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isTopBottomCorridor));
                        break;
                    case 1:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isTopRightCorridor));
                        break;
                    case 2:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isTopLeftCorridor));
                        break;
                }
            }
                
            if (doorOfParentToConnect.orientation == DoorOrientation.left)
            {
                randNum = Random.Range(0, 3);
                switch (randNum)
                {
                    case 0:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isRightLeftCorridor));
                        break;
                    case 1:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isBottomRightCorridor));
                        break;
                    case 2:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isTopRightCorridor));
                        break;
                }
            }
            if(doorOfParentToConnect.orientation == DoorOrientation.right)
            {
                randNum = Random.Range(0, 3);
                switch (randNum)
                {
                    case 0:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isRightLeftCorridor));
                        break;
                    case 1:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isBottomLeftCorridor));
                        break;
                    case 2:
                        roomModel = ChooseRandomModelForType(roomNodeTypes.list.Find(x => x.isTopLeftCorridor));
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
            if(door.isAvailable && !door.isConnected)
                result.Add(door);
        }
        return result;
    }

    private void PlaceEntrance(RoomNode roomNode)
    {
        RoomModel roomModel = ChooseRandomModelForType(roomNode.roomType);
        Room finalRoom = GenerateRoomUsingModel(roomModel, roomNode);
        finalRoom.IsPlaced = true;
        List<Door> doors = finalRoom.Doors;
        roomDictionary.Add(finalRoom.RoomId, finalRoom);
    }

    private Room GenerateRoomUsingModel(RoomModel roomModel, RoomNode roomNode)
    {
        Room room = new Room(roomNode.id,roomModel.id,roomModel.prefab,roomModel.roomType,roomModel.leftBottomPoint,roomModel.rightTopPoint,roomModel.leftBottomPoint,roomModel.rightTopPoint,
            roomModel.enemySpawns,roomModel.rewardSpawns,roomModel.teleporter,roomModel.playerSpawn,roomModel.enemies);
        room.ChildrenRooms = CopyListOfStrings(roomNode.children);
        room.Doors = CopyListOfDoors(roomModel.doors);
        if(roomNode.parentId == null)
        {
            room.ParentId = "";
            room.IsPrev = true;
        }
        else
            room.ParentId = roomNode.parentId;
        return room;
    }

    private List<Door> CopyListOfDoors(List<Door> doors)
    {
        List<Door> result = new List<Door>();
        foreach (Door door in doors)
        {
            Door doorToAdd = new Door(door.pos,door.orientation,door.prefab,door.wallToBuildPosition,door.wallBuildingWidthInTiles,door.wallBuildingHeigthInTiles);
            result.Add(doorToAdd);
        }
        return result;
    }

    private List<string> CopyListOfStrings(List<string> toCopy)
    {
        List<string> result = new List<string>();
        foreach(string str in toCopy)
        {
            result.Add(str);
        }
        return result;
    }
    

    private RoomModel ChooseRandomModelForType(RoomNodeType roomType)
    {
        List<RoomModel> roomModelsOfType = new List<RoomModel>();
        foreach(RoomModel roomModel in roomModelsList)
        {
            if(roomModel.roomType == roomType)
                roomModelsOfType.Add(roomModel);
        }
        if(roomModelsOfType.Count == 0)
        {    
            return null;
        }
        return roomModelsOfType[Random.Range(0,roomModelsOfType.Count)];
    }

    private void CreateRoomModelsDictionary()
    {
        roomModelsDictionary.Clear();
        for(int i =0;i<roomModelsList.Count;i++)
        {
            if (!roomModelsDictionary.ContainsKey(roomModelsList[i].id))
                roomModelsDictionary.Add(roomModelsList[i].id, roomModelsList[i]);
        }
    }
}
