using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonStructureGraph", menuName = "Scriptable Objects/Dungeon Structure/ DungeonStructureGraph")]
public class DungeonStructureGraph : ScriptableObject
{
    [HideInInspector]
    public RoomNodeTypes roomNodeTypes;
    [HideInInspector]
    public List<RoomNode> roomNodes = new List<RoomNode>();
    [HideInInspector]
    public Dictionary<string,RoomNode> roomNodeDictionary = new Dictionary<string, RoomNode>();

    [HideInInspector]
    public RoomNode startingNode = null;
    [HideInInspector]
    public RoomNode end = null;
    [HideInInspector]
    public Vector2 lineCoordinates;

    public void Awake()
    {
        roomNodeDictionary = roomNodes.ToDictionary(node => node.id, node => node);
    }
    public void OnValidate()
    {
        roomNodeDictionary = roomNodes.ToDictionary(node => node.id, node => node);
    }
    public void StartDrawingLine(RoomNode room,Vector2 coords)
    {
        startingNode = room;
        lineCoordinates = coords;
    }

    public RoomNode FindNodeById(string id)
    {
        return roomNodeDictionary.ContainsKey(id) ? roomNodeDictionary[id] : null;
    }   
    public RoomNode GetNode(RoomNodeType type)
    {
        return roomNodeDictionary.Values.FirstOrDefault(node => node.roomType == type);
    }
    public List<RoomNode> GetChildrenNodes(RoomNode parent)
    {
        if (parent?.children == null)
            return new List<RoomNode>();

        return parent.children.Select(childId => FindNodeById(childId)).ToList();
    }

}
