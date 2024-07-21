using Codice.CM.Common.Tree;
using System.Collections.Generic;

public class SimpleGraphNode
{
    public int index = -1;
    public List<SimpleGraphEdge> edges = new List<SimpleGraphEdge>();

    public bool Equals(SimpleGraphNode other)
    {
        return other == this;
    }

    public override string ToString()
    {
        return $"{index}";
    }
}

public class SimpleGraphEdge
{
    public SimpleGraphNode node1, node2;

    public override string ToString()
    {
        return $"{node1}-{node2}";
    }
}

/// <summary>
/// A graph with basic node and edge structure.
/// </summary>
public class SimpleGraph : LocalGraph<SimpleGraphNode, SimpleGraphEdge>
{

    #region ---------- The node and edge structure of the graph ----------

    public override bool AreEqual(SimpleGraphNode node1, SimpleGraphNode node2)
    {
        return node1.index == node2.index; // TODO: maybe compare the nodes directly?
    }

    public override bool AreEqual(SimpleGraphEdge edge1, SimpleGraphEdge edge2)
    {
        return edge1 == edge2;
    }

    public override IEnumerable<SimpleGraphEdge> GetEdgesOf(SimpleGraphNode node)
    {
        return node.edges;
    }

    public override SimpleGraphNode GetNode1Of(SimpleGraphEdge edge)
    {
        return edge.node1;
    }

    public override SimpleGraphNode GetNode2Of(SimpleGraphEdge edge)
    {
        return edge.node2;
    }

    #endregion ---------- The node and edge structure of the graph ----------


    /// <summary>
    /// Creates a new graph with #numVertices vertices. Numbering them from 0 to numVertices-1,
    /// every (i,j) tuple in the edges list corresponds to an edge from node i to node j.
    /// </summary>
    /// <param name="numVertices"> The number of vertices </param>
    /// <param name="edges"> Tuples of vertex indices representing the edges </param>
    /// <returns> The root node (with index 0) </returns>
    public SimpleGraphNode CreateGraph(int numVertices, IEnumerable<(int, int)> edges)
    {
        SimpleGraphNode[] nodes = new SimpleGraphNode[numVertices];
        for (int i = 0; i < numVertices; i++)
        {
            nodes[i] = new SimpleGraphNode { index = i };
        }
        foreach (var (i, j) in edges)
        {
            SimpleGraphEdge edge = new SimpleGraphEdge { node1 = nodes[i], node2 = nodes[j] };
            nodes[i].edges.Add(edge);
            nodes[j].edges.Add(edge);
        }

        return nodes[0];
    }

    /// <summary>
    /// Creates a new SIMPLE graph according to the incidence matrix.
    /// </summary>
    /// <param name="numVertices"> The number of vertices </param>
    /// <param name="edges"> Tuples of vertex indices representing the edges </param>
    /// <returns> The root node (with index 0) </returns>
    public SimpleGraphNode CreateGraph(int[][] matrix)
    {
        int n = matrix.GetLength(0);
        int m = matrix.GetLength(1);
        if (m != n)
            throw new System.Exception(
                $"The graph matrix should be a square matrix, but it is {n}x{m}");

        List<(int, int)> edges = new List<(int, int)>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (matrix[i][j] != 0)
                {
                    edges.Add((i, j));
                }
            }
        }

        return CreateGraph(n, edges);
    }
}