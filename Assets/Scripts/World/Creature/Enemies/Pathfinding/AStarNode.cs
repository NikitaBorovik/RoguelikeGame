using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : IComparable<AStarNode>
{
    private int x;
    private int y;
    private int gCost;
    private int hCost;
    private int fCost;
    private AStarNode parent;

    public int GCost { get => gCost; set => gCost = value; }
    public int HCost { get => hCost; set => hCost = value; }
    public int Y { get => y; set => y = value; }
    public int X { get => x; set => x = value; }
    public int FCost { get => GCost + HCost; }
    public AStarNode Parent { get => parent; set => parent = value; }

    

    public AStarNode(int x, int y)
    {
        this.X = x;
        this.Y = y;
        GCost = 0;
        HCost = 0;
        Parent = null;
    }
    public int CompareTo(AStarNode other)
    {
        int comp = FCost.CompareTo(other.FCost);
        if (comp == 0)
            comp = HCost.CompareTo(other.HCost);
       
        return comp;
    }
}
