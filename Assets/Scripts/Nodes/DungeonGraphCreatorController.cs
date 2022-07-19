using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections.Generic;

public class DungeonGraphCreatorController : EditorWindow
{
    private GUIStyle nodeStyle;
    private GUIStyle nodeStyleSelected;
    private int paddingSize = 25;
    private int borderSize = 15;
    private float width = 180f;
    private float height = 90f;
    private static DungeonStructureGraph currentGraph;
    private RoomNodeTypes roomTypes;
    private RoomNode curRoom = null;
    private const float lineWidth = 3f;
    private Vector2 gridOffset;
    private Vector2 graphDelta;
    private const float bigGridSize = 100f;
    private const float smallGridSize = 25f;

    [MenuItem("Dungeon Graph Creator", menuItem = "Window/Dungeon/Dungeon Graph Creator")]

    private static void OpenTheWindow()
    {
       GetWindow<DungeonGraphCreatorController>("Room Node Graph Creator");
    }
    
    /// <summary>
    /// opens the Room Node Graph Creator is a dungeon structure graph asset is double clicked in the inspector
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    /// 

    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID,int line)
    {
        DungeonStructureGraph graph = EditorUtility.InstanceIDToObject(instanceID) as DungeonStructureGraph;
        if (graph != null)
        {
            OpenTheWindow();
            currentGraph = graph;
            return true;
        }
        return false;
    }
    
    private void OnGUI()
    {
        if(currentGraph != null)
        {
            DrawGrid(smallGridSize, 0.25f, Color.white);
            DrawGrid(bigGridSize, 0.35f, Color.white);
            DrawLine();
            ProcessEvents(Event.current);
            DrawConnectedLines();
            DrawRoomNodes();
        }
        if(GUI.changed)
            Repaint();
    }

    private void DrawGrid(float gridS, float opacity, Color color)
    {
        Handles.color = new Color(color.r,color.g,color.b,opacity);
        int vertLines = Mathf.CeilToInt((gridS + position.width) / gridS);
        int horLines = Mathf.CeilToInt((gridS + position.height) / gridS);
        gridOffset += graphDelta / 2;
        Vector3 offset = new Vector3(gridOffset.x % gridS, gridOffset.y % gridS, 0);
        for(int i = 0; i < vertLines; i++)
            Handles.DrawLine(new Vector3(gridS * i, -gridS, 0) + offset, new Vector3(gridS * i, position.height + gridS, 0) + offset);
        for (int i = 0; i < horLines; i++)
            Handles.DrawLine(new Vector3(-gridS, gridS * i, 0) + offset, new Vector3(position.width+gridS, gridS * i, 0) + offset);
    }

    private void DrawConnectedLines()
    {
        foreach(RoomNode room in currentGraph.roomNodes)
        {
            if(room.children.Count > 0)
            {
                foreach(string id in room.children)
                {
                    if (currentGraph.roomNodeDictionary.ContainsKey(id))
                    {
                        ConnectLine(room, currentGraph.roomNodeDictionary[id]);
                        GUI.changed = true;
                    }     
                }
            }
        }
    }
    

    private void ConnectLine(RoomNode startNode, RoomNode endNode)
    {
        Vector3 start = startNode.rect.center;
        Vector3 end = endNode.rect.center;
        Handles.DrawBezier(start, end, start, end, Color.white, null, lineWidth);
        DrawArrow(start, end);
        GUI.changed = true;
    }

    private void DrawArrow(Vector2 start, Vector2 end)
    {

        Vector2 mid = (start + end) / 2;
        Vector2 arrowDirection = end - start;
        Vector2 arrowPoint1pos = mid - new Vector2(-arrowDirection.y, arrowDirection.x).normalized * 7;
        Vector2 arrowPoint2pos = mid + new Vector2(-arrowDirection.y, arrowDirection.x).normalized * 7;
        Vector2 arrowTopPosition = mid + arrowDirection.normalized * 7;
        Handles.DrawBezier(arrowPoint1pos, arrowTopPosition, arrowPoint1pos, arrowTopPosition, Color.white, null, lineWidth);
        Handles.DrawBezier(arrowPoint2pos, arrowTopPosition, arrowPoint2pos, arrowTopPosition, Color.white, null, lineWidth);
        GUI.changed = true;
    }  

    private void DrawLine()
    {
        if(currentGraph.lineCoordinates != Vector2.zero)
        {
            Vector3 start = currentGraph.startingNode.rect.center;
            Vector3 end = currentGraph.lineCoordinates;
            Handles.DrawBezier(start, end, start, end, Color.white, null, lineWidth);
           
        }
            
    }

    

    /// <summary>
    /// Draw room nodes in the window
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void DrawRoomNodes()
    {
        foreach(RoomNode roomNode in currentGraph.roomNodes)
        {
            if(!roomNode.isActive)
                roomNode.Draw(nodeStyle);
            else
                roomNode.Draw(nodeStyleSelected);
        }
        GUI.changed = true;
    }
    private RoomNode GetRoomUnderTheMouse(Event current)
    {
        for(int i = 0;i < currentGraph.roomNodes.Count;i++)
        {
            if (currentGraph.roomNodes[i].rect.Contains(current.mousePosition))
                return currentGraph.roomNodes[i];
        }
        return null;
    }
    private void RemoveLinksFromActive()
    {
        foreach(RoomNode room in currentGraph.roomNodes)
        {
            if (room.isActive)
            {
                if(room.parentId != null)
                {
                    currentGraph.GetNode(room.parentId).RemoveChild(room.id);
                }
                foreach(String id in room.children)
                {
                    currentGraph.GetNode(id).RemoveParent();
                }
                room.RemoveParent();
                room.RemoveChildren();
            }
        }
        GUI.changed = true;
    }
    public void RemoveLinksFromActiveById(string id)
    {
        RoomNode room = currentGraph.GetNode(id);
            if (room.isActive)
            {
                if (room.parentId != null)
                {
                    currentGraph.GetNode(room.parentId).RemoveChild(room.id);
                }
                foreach (String childId in room.children)
                {
                    currentGraph.GetNode(childId).RemoveParent();
                }
                room.RemoveParent();
                room.RemoveChildren();
            GUI.changed = true;
        }
    }
    private void DeleteSelectedNodes()
    {
        Queue<RoomNode> deletionQueue = new Queue<RoomNode>();
        foreach(RoomNode room in currentGraph.roomNodes)
        {
            if(room.isActive && !room.roomType.isEntrance)
            {
                deletionQueue.Enqueue(room);
                RemoveLinksFromActiveById(room.id);
            }
        }
        while(deletionQueue.Count > 0)
        {
            RoomNode toDel = deletionQueue.Dequeue();
            currentGraph.roomNodeDictionary.Remove(toDel.id);
            currentGraph.roomNodes.Remove(toDel);
            DestroyImmediate(toDel,true);
            AssetDatabase.SaveAssets();
        }
    }
    /// <summary>
    /// process dungeon graph events
    /// </summary>
    /// <param name="current"></param>
    private void ProcessEvents(Event current)
    {
        graphDelta = Vector2.zero;
        curRoom = GetRoomUnderTheMouse(current);
        if(curRoom == null || currentGraph.startingNode != null)
            ProcessGraphEvents(current);
        else
            ProcessNodeEvents(curRoom, current);
    }
    
    private void ProcessGraphEvents(Event current)
    {
        switch(current.type)
        {
            case EventType.MouseDown:
                MouseDownEvent(current);
                break;
            case EventType.MouseUp:
                MouseUpEvent(current);
                break;
            case EventType.MouseDrag:
                MouseDragEvent(current);
                break;
            default:
                break;
        }
       
    }
    private void ProcessNodeEvents(RoomNode curNode,Event current)
    {
        curNode.Proceed(current);
    }
    private void MouseDownEvent(Event current)
    {
        switch (current.button)
        {
            case 1:
                ShowContextMenu(current.mousePosition);
                break;
            case 0:
                currentGraph.lineCoordinates = Vector2.zero;
                currentGraph.startingNode = null;
                foreach(RoomNode room in currentGraph.roomNodes)
                {
                    if(room.isActive)
                    {
                        room.isActive = false;
                        GUI.changed = true;
                    }
                }
                GUI.changed = true;
                break;
            default:
                break;
        }
    }
    private void MouseUpEvent(Event current)
    {
        switch (current.button)
        {
            case 1:
                if (currentGraph.startingNode != null)
                {
                    RoomNode roomNode = GetRoomUnderTheMouse(current);
                    if(roomNode != null)
                    {
                        if (currentGraph.startingNode.AddChild(roomNode.id))
                            roomNode.AddParent(currentGraph.startingNode.id);
                    }
                    
                    currentGraph.lineCoordinates = Vector2.zero;
                    currentGraph.startingNode = null;
                    GUI.changed = true;
                    
                }
                break;
            default:
                break;
        }
    }
    
    private void MouseDragEvent(Event current)
    {
        switch (current.button)
        {
            case 0:
                graphDelta = current.delta;
                for (int i = 0; i < currentGraph.roomNodes.Count; i++)
                    currentGraph.roomNodes[i].OnMouseDrag(current);
                GUI.changed = true;
                break;
            case 1:
                if(currentGraph.startingNode != null)
                {
                    currentGraph.lineCoordinates += current.delta;
                    GUI.changed = true;
                }
                break;
            default:
                break;
        }
    }

    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Room Node"), false, CreateRoomNode, mousePosition);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Select All Node"), false, SelectAllNodes);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Remove Selected Node Links"), false, RemoveLinksFromActive);
        menu.AddItem(new GUIContent("Remove Selected Node"), false, DeleteSelectedNodes);
        menu.ShowAsContext();
    }

    private void SelectAllNodes()
    {
        foreach(RoomNode room in currentGraph.roomNodes)
        {
            room.isActive = true;
        }
        GUI.changed = true;
    }

    private void CreateRoomNode(object positionObject)
    {
        CreateRoomNode(positionObject, roomTypes.list.Find(x => x.isNone));
        if(currentGraph.roomNodes.Count == 1)
        {
            CreateRoomNode(new Vector2(250f,250f), roomTypes.list.Find(x => x.isEntrance));
        }
        
    }
    private void CreateRoomNode(object positionObject, RoomNodeType type)
    {
        Vector2 position = (Vector2)positionObject;
        RoomNode roomNode = ScriptableObject.CreateInstance<RoomNode>();
        currentGraph.roomNodes.Add(roomNode);
        roomNode.Initialise(new Rect(position, new Vector2(width, height)), currentGraph, type);
        AssetDatabase.AddObjectToAsset(roomNode, currentGraph);
        AssetDatabase.SaveAssets();
        currentGraph.OnValidate();
    }

    private void OnEnable()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        nodeStyle.padding = new RectOffset(paddingSize, paddingSize, paddingSize, paddingSize);
        nodeStyle.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);
        nodeStyle.normal.textColor = Color.white;

        nodeStyleSelected = new GUIStyle();
        nodeStyleSelected.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        nodeStyleSelected.padding = new RectOffset(paddingSize, paddingSize, paddingSize, paddingSize);
        nodeStyleSelected.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);
        nodeStyleSelected.normal.textColor = Color.white;

        Selection.selectionChanged += ChangedSelection;
        roomTypes = MyResources.Instance.roomNodeTypes;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= ChangedSelection;
    }
    private void ChangedSelection()
    {
        DungeonStructureGraph dungeonStructureGraph = Selection.activeObject as DungeonStructureGraph;
        if (dungeonStructureGraph != null)
        {
            currentGraph = dungeonStructureGraph;
            GUI.changed = true;
        }
    }
}
