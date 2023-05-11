using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class RoomNode : ScriptableObject
{
    public string id;
    public string parentId;
    public List<string> children = new List<string>();
    [HideInInspector]
    public DungeonStructureGraph dungeonStructureGraph;
    public RoomNodeType roomType;
    public RoomNodeTypes roomNodeTypes;
    [HideInInspector]
    public bool isActive = false;
    [HideInInspector]
    public bool isDragging = false;

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
    public void Initialise(DungeonStructureGraph graph, RoomNodeType roomNodeType)
    {
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.dungeonStructureGraph = graph;
        this.roomType = roomNodeType;
        roomNodeTypes = MyResources.Instance.roomNodeTypes;
    }
#if UNITY_EDITOR
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
                    dungeonStructureGraph.FindNodeById(parentId).RemoveChild(id);
                }
                foreach (String childId in children)
                {
                    dungeonStructureGraph.FindNodeById(childId).RemoveParent();
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
#endif
    public bool AddChild(string id)
    {
       children.Add(id);
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

}
