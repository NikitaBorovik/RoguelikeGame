using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections.Generic;

public class DungeonGraphCreatorController : EditorWindow
{

    private int paddingSize = 25;
    private int borderSize = 15;
    private float width = 180f;
    private float height = 90f;
    private const float bigGridSize = 100f;
    private const float smallGridSize = 25f;
    private const float lineWidth = 3f;
    private GUIStyle nodeStyle;
    private GUIStyle nodeStyleSelected;
    private static DungeonStructureGraph currentGraph;
    private AvailableNodeTypesForRoom roomTypes;
    private DungeonGraphNode curRoom = null;
    private Vector2 gridOffset;
    private Vector2 graphDelta;
    

    [MenuItem("Dungeon Graph Creator", menuItem = "Window/Dungeon/Dungeon Graph Creator")]

    private static void OpenTheWindow()
    {
        GetWindow<DungeonGraphCreatorController>("Room Graph Creator");
    }


    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        DungeonStructureGraph graph = EditorUtility.InstanceIDToObject(instanceID) as DungeonStructureGraph;
        if (graph == null)
            return false;
        OpenTheWindow();
        currentGraph = graph;
        return true;
    }

    private void OnGUI()
    {
        if (currentGraph == null)
            return;
        if (GUI.changed)
            Repaint();
        DrawGrid(smallGridSize, 0.25f, Color.white);
        DrawGrid(bigGridSize, 0.35f, Color.white);
        DrawLine();
        ProcessEvents(Event.current);
        DrawConnectedLines();
        DrawGraphNodes();
    }

    private void DrawGrid(float gridS, float opacity, Color color)
    {
        Handles.color = new Color(color.r, color.g, color.b, opacity);
        int vertLines = Mathf.CeilToInt((gridS + position.width) / gridS);
        int horLines = Mathf.CeilToInt((gridS + position.height) / gridS);
        gridOffset += graphDelta / 2;
        Vector3 offset = new Vector3(gridOffset.x % gridS, gridOffset.y % gridS, 0);
        for (int i = 0; i < vertLines; i++)
            Handles.DrawLine(new Vector3(gridS * i, -gridS, 0) + offset, new Vector3(gridS * i, position.height + gridS, 0) + offset);
        for (int i = 0; i < horLines; i++)
            Handles.DrawLine(new Vector3(-gridS, gridS * i, 0) + offset, new Vector3(position.width + gridS, gridS * i, 0) + offset);
    }

    private void DrawConnectedLines()
    {
        foreach (DungeonGraphNode room in currentGraph.roomNodes)
        {
            if (room.children.Count > 0)
            {
                foreach (string id in room.children)
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


    private void ConnectLine(DungeonGraphNode startNode, DungeonGraphNode endNode)
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
        if (currentGraph.lineCoordinates != Vector2.zero)
        {
            Vector3 start = currentGraph.startingNode.rect.center;
            Vector3 end = currentGraph.lineCoordinates;
            Handles.DrawBezier(start, end, start, end, Color.white, null, lineWidth);

        }

    }
    private void DrawGraphNodes()
    {
        foreach (DungeonGraphNode roomNode in currentGraph.roomNodes)
        {
            if (!roomNode.isActive)
                roomNode.DrawNode(nodeStyle);
            else
                roomNode.DrawNode(nodeStyleSelected);
        }
        GUI.changed = true;
    }
    private DungeonGraphNode GetRoomUnderTheMouse(Event current)
    {
        for (int i = 0; i < currentGraph.roomNodes.Count; i++)
        {
            if (currentGraph.roomNodes[i].rect.Contains(current.mousePosition))
                return currentGraph.roomNodes[i];
        }
        return null;
    }
    private void RemoveLinksFromActive()
    {
        foreach (DungeonGraphNode room in currentGraph.roomNodes)
        {
            if (room.isActive)
            {
                if (room.parentId != null)
                {
                    currentGraph.FindNodeById(room.parentId).RemoveChild(room.id);
                }
                foreach (String id in room.children)
                {
                    currentGraph.FindNodeById(id).RemoveParent();
                }
                room.RemoveParent();
                room.RemoveChildren();
            }
        }
        GUI.changed = true;
    }
    public void RemoveLinksFromActiveById(string id)
    {
        DungeonGraphNode room = currentGraph.FindNodeById(id);
        if (room.isActive)
        {
            if (room.parentId != null)
            {
                currentGraph.FindNodeById(room.parentId).RemoveChild(room.id);
            }
            foreach (String childId in room.children)
            {
                currentGraph.FindNodeById(childId).RemoveParent();
            }
            room.RemoveParent();
            room.RemoveChildren();
            GUI.changed = true;
        }
    }
    private void DeleteSelectedNodes()
    {
        Queue<DungeonGraphNode> deletionQueue = new Queue<DungeonGraphNode>();
        foreach (DungeonGraphNode room in currentGraph.roomNodes)
        {
            if (room.isActive && !room.roomType.isEntrance)
            {
                deletionQueue.Enqueue(room);
                RemoveLinksFromActiveById(room.id);
            }
        }
        while (deletionQueue.Count > 0)
        {
            DungeonGraphNode toDel = deletionQueue.Dequeue();
            currentGraph.roomNodeDictionary.Remove(toDel.id);
            currentGraph.roomNodes.Remove(toDel);
            DestroyImmediate(toDel, true);
            AssetDatabase.SaveAssets();
        }
    }
    private void ProcessEvents(Event current)
    {
        graphDelta = Vector2.zero;
        curRoom = GetRoomUnderTheMouse(current);
        if (curRoom == null || currentGraph.startingNode != null)
            ProcessGraphEvents(current);
        else
            ProcessNodeEvents(curRoom, current);
    }

    private void ProcessGraphEvents(Event current)
    {
        switch (current.type)
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
    private void ProcessNodeEvents(DungeonGraphNode curNode, Event current)
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
                foreach (DungeonGraphNode room in currentGraph.roomNodes)
                {
                    if (room.isActive)
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
                    DungeonGraphNode roomNode = GetRoomUnderTheMouse(current);
                    if (roomNode != null && currentGraph.startingNode.AddChild(roomNode.id))
                    {
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
                if (currentGraph.startingNode != null)
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
        menu.AddItem(new GUIContent("Add node"), false, CreateRoomNode, mousePosition);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Select all"), false, SelectAllNodes);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Remove links for selection"), false, RemoveLinksFromActive);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Remove selected"), false, DeleteSelectedNodes);
        menu.ShowAsContext();
    }

    private void SelectAllNodes()
    {
        foreach (DungeonGraphNode room in currentGraph.roomNodes)
        {
            room.isActive = true;
        }
        GUI.changed = true;
    }

    private void CreateRoomNode(object positionObject)
    {
        Vector2 position = (Vector2)positionObject;
        DungeonGraphNode roomNode = ScriptableObject.CreateInstance<DungeonGraphNode>();
        currentGraph.roomNodes.Add(roomNode);
        roomNode.Initialise(new Rect(position, new Vector2(width, height)), currentGraph, roomTypes.list.Find(x => x.isNone));
        AssetDatabase.AddObjectToAsset(roomNode, currentGraph);
        AssetDatabase.SaveAssets();
        currentGraph.OnValidate();
        if (currentGraph.roomNodes.Count == 1)
        {
            position = (Vector2)positionObject;
            roomNode = ScriptableObject.CreateInstance<DungeonGraphNode>();
            currentGraph.roomNodes.Add(roomNode);
            roomNode.Initialise(new Rect(position, new Vector2(width, height)), currentGraph, roomTypes.list.Find(x => x.isEntrance));
            AssetDatabase.AddObjectToAsset(roomNode, currentGraph);
            AssetDatabase.SaveAssets();
            currentGraph.OnValidate();
        }

    }
    

    private void OnEnable()
    {
        roomTypes = MyResources.GetInstance().roomNodeTypes;
        MakeNodesStyle();
        MakeSelectedNodeStyle();
        Selection.selectionChanged += ChangedSelection;
    }
    private void MakeNodesStyle()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);
        nodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        nodeStyle.padding = new RectOffset(paddingSize, paddingSize, paddingSize, paddingSize);
        nodeStyle.normal.textColor = Color.white;
    }
    private void MakeSelectedNodeStyle()
    {
        nodeStyleSelected = new GUIStyle();
        nodeStyleSelected.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);
        nodeStyleSelected.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        nodeStyleSelected.padding = new RectOffset(paddingSize, paddingSize, paddingSize, paddingSize);
        nodeStyleSelected.normal.textColor = Color.white;
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

