using System.Collections.Generic;
using UnityEngine;


public class CreaturesPathfinding : MonoBehaviour
{
    private NodesGrid grid;
    private PriorityQueue<PathfindingNode> openList;
    private HashSet<PathfindingNode> closedList;

    private void Awake()
    {
        openList = new PriorityQueue<PathfindingNode>();
        closedList = new HashSet<PathfindingNode>();
    }
    
    public Stack<Vector3> FindPath(Vector3 startPos, Vector3 targetPos, RoomData data)
    {
        var cellStartPos = data.DrawnRoom.Grid.WorldToCell(startPos);
        var cellTargetPos = data.DrawnRoom.Grid.WorldToCell(targetPos);
        cellStartPos = cellStartPos - (Vector3Int)data.RoomModel.leftBottomPoint;
        cellTargetPos = cellTargetPos - (Vector3Int)data.RoomModel.leftBottomPoint;

        int width = data.RoomModel.rightTopPoint.x + 1  - data.RoomModel.leftBottomPoint.x;
        int heigth = data.RoomModel.rightTopPoint.y + 1 - data.RoomModel.leftBottomPoint.y;

        grid = new NodesGrid(width, heigth);
        PathfindingNode start = grid.GetNode(Mathf.RoundToInt(cellStartPos.x), Mathf.RoundToInt(cellStartPos.y));
        PathfindingNode end = grid.GetNode(Mathf.RoundToInt(cellTargetPos.x), Mathf.RoundToInt(cellTargetPos.y));

        openList.Clear();
        closedList.Clear();

        openList.Enqueue(start);

        while (openList.Count > 0)
        {
            PathfindingNode currentNode = openList.Dequeue();
            if (currentNode == end)
            {
                return GetPath(start, end,data);
            }
            closedList.Add(currentNode);

            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    PathfindingNode neighbour = GetNeighbour(currentNode.X + i, currentNode.Y + j,data);
                    if(neighbour != null)
                    {
                        int additionalGridWeigth = data.DrawnRoom.GridTilesPriorityWeigths[neighbour.X, neighbour.Y];
                        int recalculatedGCost = GetDistBetweenNodes(currentNode, neighbour) + currentNode.G + additionalGridWeigth;
                        if (!openList.Contains(neighbour) || recalculatedGCost < neighbour.G)
                        {
                            neighbour.H = GetDistBetweenNodes(neighbour, end);
                            neighbour.G = recalculatedGCost;
                            neighbour.Parent = currentNode;
                            if (!openList.Contains(neighbour))
                                openList.Enqueue(neighbour);
                        }
                        
                    }
                }
            }
            
        }

        return null;
    }

    private Stack<Vector3> GetPath(PathfindingNode startNode, PathfindingNode endNode, RoomData data)
    {
        PathfindingNode currentNode = endNode;
        Stack<Vector3> path = new Stack<Vector3>();
        while (currentNode != startNode)
        {
            Vector3 worldPosCell = data.DrawnRoom.Grid.CellToWorld(new Vector3Int(currentNode.X + data.RoomModel.leftBottomPoint.x, currentNode.Y + data.RoomModel.leftBottomPoint.y, 0));
            worldPosCell += data.DrawnRoom.Grid.cellSize / 2f;
            currentNode = currentNode.Parent;
            path.Push(worldPosCell);
        }
        return path;
    }

    private int GetDistBetweenNodes(PathfindingNode firstNode, PathfindingNode secondNode)
    {
        int diffX = firstNode.X - secondNode.X;
        int diffY = firstNode.Y - secondNode.Y;

        int xAxisLen = Mathf.Abs(diffX);
        int yAxisLen = Mathf.Abs(diffY);

        if (xAxisLen > yAxisLen)
        {
            return 2 * yAxisLen + (xAxisLen - yAxisLen);
        }
        else
        {
            return 2 * xAxisLen + (yAxisLen - xAxisLen);
        }
        
    }
    
    private PathfindingNode GetNeighbour(int neighbourX, int neighbourY, RoomData data)
    {
        if (isInsideGrid(neighbourX, neighbourY, data))
        {
            PathfindingNode result = grid.GetNode(neighbourX, neighbourY);
            int additionalGridWeigth = data.DrawnRoom.GridTilesPriorityWeigths[neighbourX, neighbourY];
            if (additionalGridWeigth == 0)
                return null;
            if (!closedList.Contains(result))
                return result;
            return null;
        }
        return null;
    }
    private bool isInsideGrid(int x, int y, RoomData data)
    {
        if (x < 0 || x >= data.RoomModel.rightTopPoint.x - data.RoomModel.leftBottomPoint.x ||
            y < 0 || y >= data.RoomModel.rightTopPoint.y - data.RoomModel.leftBottomPoint.y)
        {
            return false;
        }
        return true;
    }
    private class NodesGrid
    {
        public PathfindingNode[,] grid;
        private readonly int width;
        private readonly int height;

        public NodesGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = InitializeGrid(width, height);
        }

        private PathfindingNode[,] InitializeGrid(int width, int height)
        {
            PathfindingNode[,] grid = new PathfindingNode[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = new PathfindingNode(i, j);
                }
            }
            return grid;
        }

        public PathfindingNode GetNode(int x, int y)
        {
            return (x < width && y < height) ? grid[x, y] : null;
        }
    }
}
