using System.Collections;
using System.Collections.Generic;
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

#if UNITY_EDITOR
    [HideInInspector]
    public RoomNode startingNode = null;
    [HideInInspector]
    public RoomNode end = null;
    [HideInInspector]
    public Vector2 lineCoordinates;

    public void Awake()
    {
        LoadDictionaryOfNodes();
    }
    public void OnValidate()
    {
        LoadDictionaryOfNodes();
    }

    private void LoadDictionaryOfNodes()
    {
        roomNodeDictionary.Clear();
        foreach (RoomNode node in roomNodes)
        {
            roomNodeDictionary[node.id] = node;
        }
    }
    public void StartDrawingLine(RoomNode room,Vector2 coords)
    {
        startingNode = room;
        lineCoordinates = coords;
    }

    public RoomNode GetNode(string id)
    {
        if(roomNodeDictionary.TryGetValue(id,out RoomNode room))
            return room;
        return null;
    }    
#endif
}
