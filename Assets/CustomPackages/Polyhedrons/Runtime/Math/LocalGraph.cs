using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// In a local graph structure you have access to local information, e.g. neighbors of the current vertex,
/// unlike global information like the number of vertices, edges, etc.
/// </summary>
/// <typeparam name="GraphNode"></typeparam>
/// <typeparam name="GraphEdge"></typeparam>
abstract public class LocalGraph<GraphNode, GraphEdge>
{

    abstract public bool AreEqual(GraphNode node1, GraphNode node2);

    abstract public bool AreEqual(GraphEdge edge1, GraphEdge edge2);

    abstract public IEnumerable<GraphEdge> GetEdgesOf(GraphNode node);

    abstract public GraphNode GetNode1Of(GraphEdge edge);

    abstract public GraphNode GetNode2Of(GraphEdge edge);

    virtual public GraphNode GetOtherNodeOf(GraphEdge edge, GraphNode excludeNode)
    {
        GraphNode graphNode = GetNode1Of(edge);
        return AreEqual(graphNode, excludeNode) ? GetNode2Of(edge) : graphNode;
    }
}
