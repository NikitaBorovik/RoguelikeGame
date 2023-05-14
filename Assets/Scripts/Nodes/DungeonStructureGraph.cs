using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonStructureGraph", menuName = "Scriptable Objects/Dungeon Structure/ DungeonStructureGraph")]
public class DungeonStructureGraph : ScriptableObject
{
    [HideInInspector]
    public AvailableNodeTypesForRoom roomNodeTypes;
    [HideInInspector]
    public List<DungeonGraphNode> roomNodes = new List<DungeonGraphNode>();
    [HideInInspector]
    public Dictionary<string,DungeonGraphNode> roomNodeDictionary = new Dictionary<string, DungeonGraphNode>();

    [HideInInspector]
    public DungeonGraphNode startingNode = null;
    [HideInInspector]
    public DungeonGraphNode end = null;
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
    public void StartDrawingLine(DungeonGraphNode room,Vector2 coords)
    {
        startingNode = room;
        lineCoordinates = coords;
    }

    public DungeonGraphNode FindNodeById(string id)
    {
        return roomNodeDictionary.ContainsKey(id) ? roomNodeDictionary[id] : null;
    }   
    public DungeonGraphNode GetNode(NodeTypeForRoom type)
    {
        return roomNodeDictionary.Values.FirstOrDefault(node => node.roomType == type);
    }
    public List<DungeonGraphNode> GetChildrenNodes(DungeonGraphNode parent)
    {
        if (parent?.children == null)
            return new List<DungeonGraphNode>();

        return parent.children.Select(childId => FindNodeById(childId)).ToList();
    }

}
