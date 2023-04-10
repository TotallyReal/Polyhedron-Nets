using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

/// <summary>
/// Implement this class to run a DFS search on the graph.
/// The "GraphNode" elements should be able to return the edges connecting to them (GetEdgesOf),
/// and the "GraphEdge" elements the 2 nodes connected to them (GetNode1Of, GetNode2Of).
/// </summary>
/// <typeparam name="GraphNode"></typeparam>
/// <typeparam name="GraphEdge"></typeparam>
abstract public class GraphStructure<GraphNode, GraphEdge> where GraphNode : class
{

    #region ---------- The node and edge structure of the graph ----------
    abstract public IEnumerable<GraphEdge> GetEdgesOf(GraphNode node);

    abstract public GraphNode GetNode1Of(GraphEdge edge);

    abstract public GraphNode GetNode2Of(GraphEdge edge);

    virtual public GraphNode GetOtherNodeOf(GraphEdge edge, GraphNode excludeNode)
    {
        GraphNode graphNode = GetNode1Of(edge);
        if (graphNode != excludeNode)
            return graphNode;
        return GetNode2Of(edge);
    }

    #endregion

    #region ---------- DFS ----------

    /// <summary>
    /// When running DFS and going over an edge e:v-&gt;u there are several possibilities
    /// <list type="number">
    /// <item>
    /// <description>For some reason, u is just null ...</description>
    /// </item>
    /// <item>
    /// <description>
    /// The edge leads back to the parent(namely, we used u-&gt;v to discover v).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The edge leads to an already visited node (and in particular it is part of a simple cycle).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// This is the first time we discover u, and we haven't run the DFS on u
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// This is the first time we discover u, and we have just finished running the DFS on u.
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// 0. For some reason, u is just null ...
    /// 1. The edge leads back to the parent(namely, we used u-&gt;v to discover v).
    /// 2. The edge leads to an already visited node (and in particular it is part of a simple cycle).
    /// 3. This is the first time we discover u, and we haven't run the DFS on u
    /// 4. This is the first time we discover u, and we have just finished running the DFS on u
    /// </para>
    /// </summary>
    public enum EdgeTravelType
    {
        NULL,
        FIRST_DISCOVER,
        AFTER_DISCOVER,
        TO_PARENT,
        ALREADY_DISCOVERED
    }

    public delegate void EdgeTravel(
        GraphNode fromNode, int fromPosition, int fromSmallestVisited,
        GraphNode toNode, int toPosition, int toSmallestVisited,
        GraphEdge edge, EdgeTravelType travelType);

    public void EmptyEdgeTravel(
        GraphNode fromNode, int fromPosition, int fromSmallestVisited,
        GraphNode toNode, int toPosition, int toSmallestVisited,
        GraphEdge edge, EdgeTravelType travelType) { }

    /// <summary>
    /// Runs a DFS on the graph starting at the given root node.
    /// When travelling along an edge, calls the callback method with an indication for the type of travel. See EdgeTravelType above.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="callback"></param>
    public void DFS(GraphNode root, EdgeTravel callback = null)
    {
        Dictionary<GraphNode, (int, int)> nodePositions = new Dictionary<GraphNode, (int, int)> { { root, (0, 0) } };
        _DFS(root, 0, null, nodePositions, callback);
    }

    /// <summary>
    /// Runs a DFS on the graph. The process continues at the currentNode with the currentPosition,
    /// and assumes that we got to this node through the parentNode. The positions of all the
    /// previously visited nodes are in nodePositions. Every time the algorithm travels through an
    /// edges, it calls the callback method.
    /// </summary>
    private int _DFS(
        GraphNode currentNode, int currentPosition, GraphNode parentNode, 
        Dictionary<GraphNode, (int, int)> nodePositions, EdgeTravel callback = null
    )
    {
        callback = callback ?? EmptyEdgeTravel;

        GraphNode nextNode;
        int nextPosition = -1;
        int smallestVisitedNodeIndex = currentPosition;
        int nextSmallestVisitedNodeIndex = -1;

        void Callback(GraphEdge edge, EdgeTravelType travelType)
        {
            callback(
                currentNode, currentPosition, smallestVisitedNodeIndex, 
                nextNode, nextPosition, nextSmallestVisitedNodeIndex, 
                edge, travelType);
        }

        foreach (GraphEdge edge in GetEdgesOf(currentNode))
        {
            nextNode = GetOtherNodeOf(edge, currentNode);
            nextPosition = -1;
            nextSmallestVisitedNodeIndex = -1;

            if (nextNode == null)
            {
                Callback(edge, EdgeTravelType.NULL);
                continue;
            }

            if (!nodePositions.TryGetValue(nextNode, out (int position, int smallestIndex) nextNodeParameters))
            {
                // The edge leads to a new node, add it, and assign its position
                nextPosition = nodePositions.Count;
                nodePositions.Add(nextNode, (nextPosition, nextPosition));
                Callback(edge, EdgeTravelType.FIRST_DISCOVER);

                nextSmallestVisitedNodeIndex = _DFS(nextNode, nextPosition, currentNode, nodePositions, callback);
                Callback(edge, EdgeTravelType.AFTER_DISCOVER);

                smallestVisitedNodeIndex = Mathf.Min(smallestVisitedNodeIndex, nextSmallestVisitedNodeIndex);
                nodePositions[currentNode] = (currentPosition, smallestVisitedNodeIndex);
                continue;
            }

            nextPosition = nextNodeParameters.position;
            nextSmallestVisitedNodeIndex = nextNodeParameters.smallestIndex;

            // The edge back to the parent
            if (nextNode == parentNode)
            {
                Callback(edge, EdgeTravelType.TO_PARENT);
                continue;
            }

            // The edge leads to a visited node which is not the parent
            smallestVisitedNodeIndex = Mathf.Min(smallestVisitedNodeIndex, nextPosition);
            Callback(edge, EdgeTravelType.ALREADY_DISCOVERED);

        }

        return smallestVisitedNodeIndex;
    }

    #endregion

    /// <summary>
    /// Find and returns all the "strongly connected components" in the connected component of the given root.
    /// A connected component in this udirected graph is defined to be a maximal set of nodes such that each two nodes 
    /// belong to a simple cycle. Alternatively, an edge does not belong to two vertices in the same component if
    /// and only if removing it will cause the connected component of the root to become disconnected.
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public Dictionary<GraphNode, int> GetStronglyConnectedComponents(GraphNode root)
    {
        List<DirectedEdge> directedEdges = GetDirectedEdgesDFS(root);
        HashSet<GraphEdge> separators = new HashSet<GraphEdge>();
        separators.AddRange(from directedEdge in directedEdges where directedEdge.seperatorEdge select directedEdge.edge);
        /*HashSet<GraphEdge> edges = new HashSet<GraphEdge>();
        edges.AddRange(from directedEdge in directedEdges select directedEdge.edge);
        Subgraph <GraphNode, GraphEdge> subgraph = new Subgraph<GraphNode, GraphEdge>(this, (edge) => edges.Contains(edge));*/

        Dictionary<GraphNode, int> componentIndex = new Dictionary<GraphNode, int>();
        componentIndex[root] = 0;
        int index = 0;

        void ComponentEdgeTravel(
            GraphNode fromNode, int fromPosition, int fromSmallestVisited,
            GraphNode toNode, int toPosition, int toSmallestVisited,
            GraphEdge edge, EdgeTravelType travelType)
        { 
            if (travelType == EdgeTravelType.FIRST_DISCOVER)
            {
                if (separators.Contains(edge))
                {
                    index++;
                    componentIndex[toNode] = index;
                } else
                {
                    componentIndex[toNode] = componentIndex[fromNode];
                }
            }
        }

        DFS(root, ComponentEdgeTravel);
        return componentIndex;
    }

    /// <summary>
    /// Used to describe an edge in the directed tree generated by the DFS.
    /// The seperatorEdge boolean is true when the edge is not part of a cycle in the original graph.
    /// </summary>
    public struct DirectedEdge
    {
        public GraphNode parent;
        public GraphEdge edge;
        public GraphNode child;
        public bool treeEdge;
        public bool seperatorEdge;
    }

    /// <summary>
    /// Runs a DFS on the graph starting with the given node.
    /// Returns a list of the edges forming a spanning tree from the root.
    /// </summary>
    /// <param name="root"></param>
    public List<DirectedEdge> GetDirectedEdgesDFS(GraphNode root)
    {
        List<DirectedEdge> directedEdges = new List<DirectedEdge>();

        // with callback
        void SimpleEdgeTravel(
            GraphNode fromNode, int fromPosition, int fromSmallestVisited,
            GraphNode toNode, int toPosition, int toSmallestVisited,
            GraphEdge edge, EdgeTravelType travelType)
        {
            switch (travelType)
            {
                case EdgeTravelType.NULL:
                    directedEdges.Add(new DirectedEdge // TODO adding this edge twice - fix it
                    {
                        parent = fromNode,
                        edge = edge,
                        child = toNode,
                        treeEdge = false,
                        seperatorEdge = false
                    });
                    break;
                case EdgeTravelType.ALREADY_DISCOVERED:
                    if (fromPosition < toPosition)
                    {
                        directedEdges.Add(new DirectedEdge
                        {
                            parent = fromNode,
                            edge = edge,
                            child = toNode,
                            treeEdge = false,
                            seperatorEdge = false
                        });
                    }
                    break;
                case EdgeTravelType.AFTER_DISCOVER:
                    directedEdges.Add(new DirectedEdge
                    {
                        parent = fromNode,
                        edge = edge,
                        child = toNode,
                        treeEdge = true,
                        seperatorEdge = (fromPosition < toSmallestVisited)
                    });
                    break;
            }
        }


        Dictionary<GraphNode, (int, int)> nodePositions = new Dictionary<GraphNode, (int, int)>{ { root, (0,0) } };
        DFS(root, SimpleEdgeTravel);
        return directedEdges;
    }

    #region To Delete
    /// <summary>
    /// Always assume that currentNode is in nodePositions
    /// </summary>
    private int OldDFS(
        GraphNode currentNode, int currentPosition, GraphNode parent, List<DirectedEdge> treeEdges,
        Dictionary<GraphNode, int> nodePositions
    )
    {
        int smallestVisitedNodeIndex = currentPosition;

        foreach (GraphEdge edge in GetEdgesOf(currentNode))
        {
            GraphNode nextNode = GetOtherNodeOf(edge, currentNode);

            if (nextNode == null)
            {
                treeEdges.Add(new DirectedEdge // TODO adding this edge twice - fix it
                {
                    parent = currentNode,
                    edge = edge,
                    child = nextNode,
                    treeEdge = false,
                    seperatorEdge = false
                });
                continue;
            }

            // The edge back to the parent
            if (nextNode == parent)
            {
                continue;
            }

            // The edge leads to a visited node
            if (nodePositions.TryGetValue(nextNode, out int nextPosition))
            {
                if (currentPosition < nextPosition)
                {
                    treeEdges.Add(new DirectedEdge
                    {
                        parent = currentNode,
                        edge = edge,
                        child = nextNode,
                        treeEdge = false,
                        seperatorEdge = false
                    });
                }
                smallestVisitedNodeIndex = Mathf.Min(smallestVisitedNodeIndex, nextPosition);
                continue;
            }

            // The edge leads to a new node
            nextPosition = nodePositions.Count;
            nodePositions.Add(nextNode, nextPosition);
            int nextSmallestVisitedNodeIndex = OldDFS(nextNode, nextPosition, currentNode, treeEdges, nodePositions);
            treeEdges.Add(new DirectedEdge
            {
                parent = currentNode,
                edge = edge,
                child = nextNode,
                treeEdge = true,
                seperatorEdge = (currentPosition < nextSmallestVisitedNodeIndex)
            });
            //Debug.Log($"Adding tree edge from {currentNode} to {nextNode} where seperator={currentPosition < nextSmallestVisitedNodeIndex}");
            smallestVisitedNodeIndex = Mathf.Min(smallestVisitedNodeIndex, nextSmallestVisitedNodeIndex);
        }

        return smallestVisitedNodeIndex;
    }
    #endregion
}

public class Subgraph<GraphNode, GraphEdge> : GraphStructure<GraphNode, GraphEdge> where GraphNode : class
{
    public delegate bool ContainsEdge(GraphEdge edge);

    private readonly GraphStructure<GraphNode, GraphEdge> graph;
    private readonly ContainsEdge filter;

    public Subgraph(GraphStructure<GraphNode, GraphEdge> graph, ContainsEdge filter)
    {
        this.graph = graph;
        this.filter = filter;
    }

    public override IEnumerable<GraphEdge> GetEdgesOf(GraphNode node)
    {
        return graph.GetEdgesOf(node).Where(edge => filter(edge));
    }

    public override GraphNode GetNode1Of(GraphEdge edge)
    {
        return graph.GetNode1Of(edge);
    }

    public override GraphNode GetNode2Of(GraphEdge edge)
    {
        return graph.GetNode2Of(edge);
    }
}   


