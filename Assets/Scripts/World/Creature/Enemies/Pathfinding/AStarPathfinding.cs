using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AStarPathfinding : MonoBehaviour
{
    private NodesGrid grid;
    private PriorityQueue<AStarNode> openList;
    private HashSet<AStarNode> closedList;

    private void Awake()
    {
        openList = new PriorityQueue<AStarNode>();
        closedList = new HashSet<AStarNode>();
    }
    
    public Stack<Vector3> FindPath(Vector3Int startPos, Vector3Int targetPos, Room room)
    {
        startPos -= (Vector3Int)room.RoomModel.leftBottomPoint;
        targetPos -= (Vector3Int)room.RoomModel.leftBottomPoint;
        Debug.Log("Start:" + startPos);
        Debug.Log("Target:" + targetPos);

        grid = new NodesGrid(room.RoomModel.rightTopPoint.x - room.RoomModel.leftBottomPoint.x + 1, room.RoomModel.rightTopPoint.y - room.RoomModel.leftBottomPoint.y + 1);

        //foreach (AStarNode node in grid.grid)
        //{
        //    Debug.Log(node.X + " " + node.Y);
        //}

        AStarNode startNode = grid.GetNode(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y));
        Debug.Log(startNode);
        AStarNode targetNode = grid.GetNode(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y));
        Debug.Log(targetNode);

        openList.Clear();
        closedList.Clear();

        openList.Enqueue(startNode);

        while (openList.Count > 0)
        {
            AStarNode currentNode = openList.Dequeue();
            Debug.Log("FCost:" + currentNode.FCost);
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
                        int recalculatedGCost = GetDistance(currentNode, neighbour) + currentNode.GCost + additionalGridWeigth;
                        if (!openList.Contains(neighbour) || recalculatedGCost < neighbour.GCost)
                        {
                            neighbour.GCost = recalculatedGCost;
                            neighbour.HCost = GetDistance(neighbour, targetNode);
                            neighbour.Parent = currentNode;
                            if (!openList.Contains(neighbour))
                                openList.Enqueue(neighbour);
                            Debug.Log(neighbour.FCost);
                        }
                        
                    }
                }
            }
            
        }

        return null;
    }

    private Stack<Vector3> GetPath(AStarNode startNode, AStarNode endNode, Room room)
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

    private int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.X - nodeB.X);
        int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
    
    private AStarNode GetNeighbour(int neighbourX, int neighbourY, Room room)
    {
        if(neighbourX < 0 || neighbourX >= room.RoomModel.rightTopPoint.x - room.RoomModel.leftBottomPoint.x ||
            neighbourY < 0 || neighbourY >= room.RoomModel.rightTopPoint.y - room.RoomModel.leftBottomPoint.y)
        {
            return null;
        }
        AStarNode result = grid.GetNode(neighbourX, neighbourY);
        int additionalGridWeigth = room.DrawnRoom.GridTilesPriorityWeigths[neighbourX, neighbourY];
        if (additionalGridWeigth == 0)
            return null;
        if (!closedList.Contains(result))
            return result;
        return null;
    }
    
}
