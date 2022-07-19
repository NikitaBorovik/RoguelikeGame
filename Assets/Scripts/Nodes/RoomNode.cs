using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class RoomNode : ScriptableObject
{
    //[HideInInspector]
    public string id;
    //[HideInInspector]
    public string parentId;
    //[HideInInspector]
    public List<string> children = new List<string>();
    [HideInInspector]
    public DungeonStructureGraph dungeonStructureGraph;
    public RoomNodeType roomType;
    [HideInInspector]
    public RoomNodeTypes roomNodeTypes;
    [HideInInspector]
    public bool isActive = false;
    [HideInInspector]
    public bool isDragging = false;

#if UNITY_EDITOR
    [HideInInspector] public Rect rect;

    public void Initialise(Rect rect, DungeonStructureGraph graph, RoomNodeType roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.dungeonStructureGraph = graph;
        this.roomType = roomNodeType;
        roomNodeTypes = MyResources.Instance.roomNodeTypes;
    }

    public void Draw(GUIStyle style)
    {
        GUILayout.BeginArea(rect, style);
        EditorGUI.BeginChangeCheck();
        if(parentId != null || roomType.isEntrance)
        {
            EditorGUILayout.LabelField(roomType.typeName);
        }
        else
        {
        int selected = roomNodeTypes.list.FindIndex(x => x == roomType);
        int selection = EditorGUILayout.Popup("", selected, GetRoomTypesToDisplay());
        roomType = roomNodeTypes.list[selection];
        if(roomNodeTypes.list[selected].isCorridor && !roomNodeTypes.list[selection].isCorridor || !roomNodeTypes.list[selected].isCorridor && roomNodeTypes.list[selection].isCorridor
                || !roomNodeTypes.list[selected].isBoss && roomNodeTypes.list[selection].isBoss)
            {
                if (parentId != null)
                {
                    dungeonStructureGraph.GetNode(parentId).RemoveChild(id);
                }
                foreach (String childId in children)
                {
                    dungeonStructureGraph.GetNode(childId).RemoveParent();
                }
                RemoveParent();
                RemoveChildren();
            }
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(this);
        }
        GUILayout.EndArea();
    }

    public string[] GetRoomTypesToDisplay()
    {
        string[] result = new string[roomNodeTypes.list.Count];
        for (int i = 0; i < roomNodeTypes.list.Count; i++)
        {
            if (roomNodeTypes.list[i].displayInCreator)
            {
                result[i] = roomNodeTypes.list[i].typeName;
            }
        }
        return result;
    }

    public void Proceed(Event current)
    {
        if (current.type == EventType.MouseDown)
        {
            OnMouseDown(current);
        }
        else if (current.type == EventType.MouseUp)
        {
            OnMouseUp(current);
        }
        else if (current.type == EventType.MouseDrag)
        {
            OnMouseDrag(current);
        }
    }
    public void OnMouseDown(Event current)
    {
        switch (current.button)
        {
            case 0:
                Selection.activeObject = this;
                isActive = !isActive;
                break;
            case 1:
                dungeonStructureGraph.StartDrawingLine(this, current.mousePosition);
                break;

        }
    }
    public void OnMouseUp(Event current)
    {
        switch (current.button)
        {
            case 0:
                isDragging = false;
                break;
        }
    }
    public void OnMouseDrag(Event current)
    {
        isDragging = true;
        rect.position += current.delta;
        EditorUtility.SetDirty(this);
        GUI.changed = true;
    }
    public bool AddChild(string id)
    {
        if (ValidateChildRoom(id))
        {
            children.Add(id);
            return true;
        }
        return false;
    }

    private bool ValidateChildRoom(string id)
    {
        bool bossConnected = false;
        foreach (RoomNode room in dungeonStructureGraph.roomNodes)
        {
            if (room.roomType.isBoss && room.parentId != null)
            {
                bossConnected = true;
            }
        }
        if(dungeonStructureGraph.GetNode(id).roomType.isNone)
            return false;
        if (children.Contains(id)|| this.id == id)
            return false;
        if (dungeonStructureGraph.GetNode(id).roomType.isBoss && bossConnected)
            return false;
        if (parentId == id)
            return false;
        if(dungeonStructureGraph.GetNode(id).roomType.isCorridor && this.roomType.isCorridor)
            return false;
        if (!dungeonStructureGraph.GetNode(id).roomType.isCorridor && !this.roomType.isCorridor)
            return false;
        if (dungeonStructureGraph.GetNode(id).roomType.isCorridor && children.Count == Settings.maxChildrenCorridors)
            return false;
        if (dungeonStructureGraph.GetNode(id).roomType.isEntrance)
            return false;
        if (dungeonStructureGraph.GetNode(id).parentId != null)
            return false;
        if(dungeonStructureGraph.GetNode(id).roomType.isCorridor && dungeonStructureGraph.GetNode(id).children.Count > 0)
            return false;
        return true;
    }

    public bool AddParent(string id)
    {
        parentId = id;
        return true;
    }
    public void RemoveParent()
    {
        parentId = null;
    }
    public void RemoveChildren()
    {
        children.Clear();
    }
    public void RemoveChild(string id)
    {
        if (children.Contains(id))
        {
            children.Remove(id);
        }
    }
#endif

}
