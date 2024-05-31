using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


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
/// 
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
    NULL,                   // edge leads to null node
    FIRST_DISCOVER,         // edge leads to a newly discovered node
    AFTER_DISCOVER,         // travelling back on an edge that led to a newly discovered node
    TO_PARENT,              // edge leads to the parent (from which this current node was discovered)
    ALREADY_DISCOVERED      // edge leads to an already discovered node
}

public class GraphAlgo<GraphNode, GraphEdge>
{

    LocalGraph<GraphNode, GraphEdge> graph;

    public GraphAlgo(LocalGraph<GraphNode, GraphEdge> graph)
    {
        Assert.IsNotNull(graph);
        this.graph = graph;
    }

    #region ---------- DFS ----------

    /// <summary>
    /// Called when DFS passes over edge: fromNode->toNode.
    /// The position arguments are the order of node discovery along the DFS process.
    /// Each node also has the smallest position it can reach, without going through its parent in the DFS process.
    /// This smallest position is updated after each time returning from one of its children, or a discovered node. 
    /// More specifically, if u->v leads to a newly discovered node v, then it generates three callbacks:
    ///     1. FIRST_DISCOVER: when it is used for the first time
    ///     2. TO_PARENT:      when from v we use the opposite edge, and
    ///     3. AFTER_DISCOVER: after finishing processing v, and going back to u. 
    ///     After the callback for 3 is called, the smallest position for u is updated.
    /// If u->v leads to a nonparent discovered node v, then update the smallest position for u from the smallest reachable position from v.
    /// </summary>
    /// <param name="fromNode"></param>
    /// <param name="fromPosition"></param>
    /// <param name="fromSmallestVisited"></param>
    /// <param name="toNode"></param>
    /// <param name="toPosition"></param>
    /// <param name="toSmallestVisited"></param>
    /// <param name="edge"></param>
    /// <param name="travelType"></param>
    public delegate void EdgeTravel(
        GraphNode fromNode, int fromPosition, int fromSmallestVisited,
        GraphNode toNode, int toPosition, int toSmallestVisited,
        GraphEdge edge, EdgeTravelType travelType);

    public static void EmptyEdgeTravel(
        GraphNode fromNode, int fromPosition, int fromSmallestVisited,
        GraphNode toNode, int toPosition, int toSmallestVisited,
        GraphEdge edge, EdgeTravelType travelType)
    { }

    /// <summary>
    /// Runs a DFS on the graph starting at the given root node.
    /// When travelling along an edge, calls the callback method with an indication for the type of travel. See EdgeTravelType above.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="callback"></param>
    public void DFS(GraphNode root, EdgeTravel callback)
    {
        Assert.IsNotNull(callback);      // If it is null, then basically this DFS does nothing.
        //callback ??= EmptyEdgeTravel;
        Dictionary<GraphNode, (int, int)> nodePositions = new Dictionary<GraphNode, (int, int)> { { root, (0, 0) } };
        _DFS(root, 0, -1, nodePositions, callback);
    }

    /// <summary>
    /// Runs a DFS on the graph. The process continues at the currentNode with the currentPosition,
    /// and assumes that we got to this node through the parentNode. The positions of all the
    /// previously visited nodes are in nodePositions. Every time the algorithm travels through an
    /// edges, it calls the callback method.
    /// </summary>
    private int _DFS(
        GraphNode currentNode, int currentPosition, int parentPosition,
        Dictionary<GraphNode, (int, int)> nodePositions, EdgeTravel callback
    )
    {

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

        foreach (GraphEdge edge in graph.GetEdgesOf(currentNode))
        {
            nextNode = graph.GetOtherNodeOf(edge, currentNode);
            nextPosition = -1;
            nextSmallestVisitedNodeIndex = -1;

            if (nextNode == null)   // TODO: should not have a null node
            {
                Callback(edge, EdgeTravelType.NULL);
                continue;
            }

            if (!nodePositions.TryGetValue(nextNode, out (int position, int smallestIndex) nextNodeParameters))
            {
                // The edge leads to a new node, add it, and assign its position
                nextPosition = nodePositions.Count;
                nodePositions.Add(nextNode, (nextPosition, nextPosition));
                nextSmallestVisitedNodeIndex = nextPosition;
                Callback(edge, EdgeTravelType.FIRST_DISCOVER);

                nextSmallestVisitedNodeIndex = _DFS(nextNode, nextPosition, currentPosition, nodePositions, callback);
                Callback(edge, EdgeTravelType.AFTER_DISCOVER);

                smallestVisitedNodeIndex = Mathf.Min(smallestVisitedNodeIndex, nextSmallestVisitedNodeIndex);
                nodePositions[currentNode] = (currentPosition, smallestVisitedNodeIndex);
                continue;
            }

            nextPosition = nextNodeParameters.position;
            nextSmallestVisitedNodeIndex = nextNodeParameters.smallestIndex;

            // The edge back to the parent
            if (nextPosition == parentPosition)
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

        public override string ToString()
        {
            if (parent is Object && child is Object)
            {
                string treeString = treeEdge ? "tree" : "";
                return $"{(parent as Object).name} -> {(child as Object).name} {treeString}";
            }

            if (edge != null)
            {
                return edge.ToString();
            }

            return base.ToString();
        }
    }

    /// <summary>
    /// Runs a DFS on the graph starting with the given node.
    /// Returns a list of DirectedEdge which appeared in the DFS, each one contains
    ///  (1) The parent-edge-child corresponding to the pass on the edge, 
    ///  (2) Whether it was used as part of the spanning tree that the DFS creates, and
    ///  (3) Is it a separating edge, namely removing it separates the connected component to 2.
    ///  
    /// Note that it is possible to have an edge from node to null (namely child==null).
    /// </summary>
    /// <param name="root">The root node for the DFS</param>
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


        //Dictionary<GraphNode, (int, int)> nodePositions = new Dictionary<GraphNode, (int, int)>{ { root, (0,0) } };
        DFS(root, SimpleEdgeTravel);
        return directedEdges;
    }

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
        List<GraphEdge> separators = new List<GraphEdge>();
        separators.AddRange(from directedEdge in directedEdges where directedEdge.seperatorEdge select directedEdge.edge);
        // TODO : maybe change separators to a set?


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
                }
                else
                {
                    componentIndex[toNode] = componentIndex[fromNode];
                }
            }
        }

        DFS(root, ComponentEdgeTravel);
        return componentIndex;
    }
}
