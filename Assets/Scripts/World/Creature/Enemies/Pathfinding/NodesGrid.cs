using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesGrid 
{
    private int width;
    private int height;
    public AStarNode[,] grid;

    public NodesGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new AStarNode[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new AStarNode(x, y);
            }
        }
    }

    public AStarNode GetNode(int x, int y)
    {
        if (x < width && y < height)
            return grid[x, y];
        else
            return null;
    }
}
