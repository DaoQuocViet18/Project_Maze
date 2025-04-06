using UnityEngine;

public class Node
{
    public Vector3Int position;
    public Node parent;
    public float gCost; // Khoảng cách từ start
    public float hCost; // Khoảng cách ước tính đến goal
    public float fCost => gCost + hCost;

    public Node(Vector3Int pos)
    {
        position = pos;
    }
} 