using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords
{
    // Start is called before the first frame update

    public int x;
    public int y;
    public Coords parent;

    public Coords(int x, int y, Coords parent)
    {
        this.x = x;
        this.y = y;
        this.parent = parent;
    }

    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public float getHeuristic(int tarPosX, int tarPosY)
    {
        return (float)Mathf.Sqrt(Mathf.Pow(this.x - tarPosX, 2) + Mathf.Pow(this.y - tarPosY, 2));
    }
}
