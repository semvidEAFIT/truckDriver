using UnityEngine;

public class TSPCase
{
    private Vector2[] nodes;
    private int[,] distanceMatrix;
    private Vector2 spaceDimensions;

    public Vector2 SpaceDimensions
    {
        get { return spaceDimensions; }
        set { spaceDimensions = value; }
    }

    public Vector2[] Nodes
    {
        get { return nodes; }
        set { nodes = value; }
    }

    public int[,] DistanceMatrix
    {
        get { return distanceMatrix; }
        set { distanceMatrix = value; }
    }

    public TSPCase(Vector2[] nodes, Vector2 spaceDimensions)
    {
        this.nodes = nodes;
        this.spaceDimensions = spaceDimensions;
        createDistanceMatrix();
    }

    private void createDistanceMatrix()
    {
        distanceMatrix = new int[nodes.Length, nodes.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int j = 0; j < nodes.Length; j++)
            {
                distanceMatrix[i, j] = (int)(Mathf.Abs(nodes[i].x - nodes[j].x) + Mathf.Abs(nodes[i].y - nodes[j].y));
            }
        }
    }
}