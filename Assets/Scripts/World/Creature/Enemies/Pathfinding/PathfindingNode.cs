using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : IComparable<PathfindingNode>
{
    private int x;
    private int y;
    private int g;
    private int h;
    private int f;
    private PathfindingNode parent;

    public int G { get => g; set => g = value; }
    public int H { get => h; set => h = value; }
    public int Y { get => y; set => y = value; }
    public int X { get => x; set => x = value; }
    public int F { get => G + H; }
    public PathfindingNode Parent { get => parent; set => parent = value; }



    public PathfindingNode(int x, int y)
    {
        X = x;
        Y = y;
        G = 0;
        H = 0;
        Parent = null;
    }
    public int CompareTo(PathfindingNode other)
    {
        int comp = F.CompareTo(other.F);
        if (comp == 0)
            comp = H.CompareTo(other.H);

        return comp;
    }
}
