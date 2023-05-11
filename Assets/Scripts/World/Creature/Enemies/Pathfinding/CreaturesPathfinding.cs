using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CreaturesPathfinding : MonoBehaviour
{
    private NodesGrid grid;
    private PriorityQueue<AStarNode> openList;
    private HashSet<AStarNode> closedList;

    private void Awake()
    {
        openList = new PriorityQueue<AStarNode>();
        closedList = new HashSet<AStarNode>();
    }
    
    public Stack<Vector3> FindPath(Vector3 startPos, Vector3 targetPos, RoomData room)
    {
        var cellStartPos = room.DrawnRoom.Grid.WorldToCell(startPos);
        var cellTargetPos = room.DrawnRoom.Grid.WorldToCell(targetPos);
        cellStartPos -= (Vector3Int)room.RoomModel.leftBottomPoint;
        cellTargetPos -= (Vector3Int)room.RoomModel.leftBottomPoint;

        grid = new NodesGrid((room.RoomModel.rightTopPoint.x - room.RoomModel.leftBottomPoint.x + 1), (room.RoomModel.rightTopPoint.y - room.RoomModel.leftBottomPoint.y + 1));
        AStarNode startNode = grid.GetNode(Mathf.RoundToInt(cellStartPos.x), Mathf.RoundToInt(cellStartPos.y));
        AStarNode targetNode = grid.GetNode(Mathf.RoundToInt(cellTargetPos.x), Mathf.RoundToInt(cellTargetPos.y));

        openList.Clear();
        closedList.Clear();

        openList.Enqueue(startNode);

        while (openList.Count > 0)
        {
            AStarNode currentNode = openList.Dequeue();
            if (currentNode == targetNode)
            {
                return GetPath(startNode, targetNode,room);
            }
            closedList.Add(currentNode);

            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    AStarNode neighbour = GetNeighbour(currentNode.X + i, currentNode.Y + j,room);
                    if(neighbour != null)
                    {
                        int additionalGridWeigth = room.DrawnRoom.GridTilesPriorityWeigths[neighbour.X, neighbour.Y];
                        int recalculatedGCost = GetDistBetweenNodes(currentNode, neighbour) + currentNode.GCost + additionalGridWeigth;
                        if (!openList.Contains(neighbour) || recalculatedGCost < neighbour.GCost)
                        {
                            neighbour.GCost = recalculatedGCost;
                            neighbour.HCost = GetDistBetweenNodes(neighbour, targetNode);
                            neighbour.Parent = currentNode;
                            if (!openList.Contains(neighbour))
                                openList.Enqueue(neighbour);
                         //   Debug.Log(neighbour.FCost);
                        }
                        
                    }
                }
            }
            
        }

        return null;
    }

    private Stack<Vector3> GetPath(AStarNode startNode, AStarNode endNode, RoomData room)
    {
        Stack<Vector3> path = new Stack<Vector3>();
        AStarNode currentNode = endNode;
        Vector3 midPos = room.DrawnRoom.Grid.cellSize / 2f;
        midPos.z = 0;
        while (currentNode != startNode)
        {
            Vector3 worldPosCell = room.DrawnRoom.Grid.CellToWorld(new Vector3Int(currentNode.X + room.RoomModel.leftBottomPoint.x, currentNode.Y + room.RoomModel.leftBottomPoint.y, 0));
            worldPosCell += midPos;
            path.Push(worldPosCell);
            currentNode = currentNode.Parent;
        }
        return path;
    }

    private int GetDistBetweenNodes(AStarNode firstNode, AStarNode secondNode)
    {
        int diffX = firstNode.X - secondNode.X;
        int diffY = firstNode.Y - secondNode.Y;

        int xAxisLen = Mathf.Abs(diffX);
        int yAxisLen = Mathf.Abs(diffY);

        if (xAxisLen > yAxisLen)
        {
            return 2 * yAxisLen + 1 * (xAxisLen - yAxisLen);
        }
        else
        {
            return 2 * xAxisLen + 1 * (yAxisLen - xAxisLen);
        }
        
    }
    
    private AStarNode GetNeighbour(int neighbourX, int neighbourY, RoomData room)
    {
        if (isInsideGrid(neighbourX, neighbourY, room))
        {
            AStarNode result = grid.GetNode(neighbourX, neighbourY);
            int additionalGridWeigth = room.DrawnRoom.GridTilesPriorityWeigths[neighbourX, neighbourY];
            if (additionalGridWeigth == 0)
                return null;
            if (!closedList.Contains(result))
                return result;
            return null;
        }
        return null;
    }
    private bool isInsideGrid(int x, int y, RoomData room)
    {
        if (x < 0 || x >= room.RoomModel.rightTopPoint.x - room.RoomModel.leftBottomPoint.x ||
            y < 0 || y >= room.RoomModel.rightTopPoint.y - room.RoomModel.leftBottomPoint.y)
        {
            return false;
        }
        return true;
    }
    private class NodesGrid
    {
        private int width;
        private int height;
        public AStarNode[,] grid;

        public NodesGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = InitializeGrid(width, height);
        }

        private AStarNode[,] InitializeGrid(int width, int height)
        {
            AStarNode[,] grid = new AStarNode[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new AStarNode(x, y);
                }
            }
            return grid;
        }

        public AStarNode GetNode(int x, int y)
        {
            if (x < width && y < height)
                return grid[x, y];
            else
                return null;
        }
    }
}
