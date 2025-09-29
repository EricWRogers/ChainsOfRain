using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AStarNode
{
    public Vector3 position;
    public float g;
    public float h;
    public float f;
    public int predecessorID;
    public List<int> adjacentPointIDs;

    public AStarNode(Vector3 _position)
    {
        position = _position;
        g = 0f;
        h = 0f;
        f = 0f;
        predecessorID = -1;
        adjacentPointIDs = new List<int>();
    }
}