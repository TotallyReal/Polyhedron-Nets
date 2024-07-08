using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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

    abstract public IEnumerable<GraphEdge> GetEdgesOf(GraphNode node);

    abstract public GraphNode GetNode1Of(GraphEdge edge);

    abstract public GraphNode GetNode2Of(GraphEdge edge);

    virtual public GraphNode GetOtherNodeOf(GraphEdge edge, GraphNode excludeNode)
    {
        GraphNode graphNode = GetNode1Of(edge);
        return (graphNode == excludeNode) ? GetNode2Of(edge) : graphNode;
    }


    #region ---------- DFS ----------

    public enum EdgeTravelType
    {
        NULL,                   // edge leads to null node
        FIRST_DISCOVER,         // edge leads to a newly discovered node
        AFTER_DISCOVER,         // travelling back on an edge that led to a newly discovered node
        TO_PARENT,              // edge leads to the parent (from which this current node was discovered)
        ALREADY_DISCOVERED      // edge leads to an already discovered node
    }


    /// <summary>
    /// Called when DFS passes over edge: fromNode->toNode.
    /// The position arguments are the order of node discovery along the DFS process.
    /// Each node also has the smallest position it can reach, without going through its parent in the DFS process.
    /// This smallest position is updated after each time returning from one of its children. More specifically, if u->v leads to a newly
    /// discovered node v, then it generates three callbacks:
    /// 1. FIRST_DISCOVER: when it is used for the first time
    /// 2. TO_PARENT:      when from v we use the opposite edge, and
    /// 3. AFTER_DISCOVER: after finishing processing v, and going back to u. 
    /// After the callback for 3 is called, the smallest position for u is updated.
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

    public void EmptyEdgeTravel(
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
        callback ??= EmptyEdgeTravel;

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
                nextSmallestVisitedNodeIndex = nextPosition;
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
    /// Creates a random spanning tree for the connected component containing the given root.
    /// </summary>
    /// <param name="root"></param>
    public List<DirectedEdge> RandomDirectedTree(GraphNode root)
    {
        List<DirectedEdge> directedEdges = GetDirectedEdgesDFS(root);

        IEnumerable<DirectedEdge> allEdges = from directedEdge in directedEdges 
                                       where directedEdge.child != null 
                                       select directedEdge;
        allEdges = MathTools.RandomList(new List<DirectedEdge>(allEdges));

        // Create a random tree by going over the edges (randomly) and form connected components.
        // Any edge that connects two distinct components, will become a tree edge.
        // We keep track over the component using the graph described by
        //      (1) firstComponent: which gives each node an initial component index, and
        //      (2) nextComponent: which keeps the parentIndex of each childIndex.
        // Two nodes are in the same componet, if they have the same first ancestor.
        List<DirectedEdge> newDirectedEdges = new List<DirectedEdge>();
        List<GraphEdge> edges = new List<GraphEdge>();

        Dictionary<GraphNode, int> firstComponent = 
            new Dictionary<GraphNode, int>{ { root, 0 } };
        int componentIndex = 1;
        Dictionary<int, int> nextComponent = new Dictionary<int, int>();
        int GetComponent(GraphNode node)
        {
            if (firstComponent.TryGetValue(node, out int index)){
                int initialIndex = index;
                while (nextComponent.TryGetValue(index, out int nextIndex))
                {
                    if (nextIndex == initialIndex)
                    {
                        Debug.LogError("Found a loop in the component graph");
                        return -1;
                    }
                    index = nextIndex;
                }
                return index;
            }
            // Haven't seen this node yet. Start a new component.
            firstComponent.Add(node, componentIndex);
            componentIndex++;
            return componentIndex - 1;
        }
        foreach (DirectedEdge directedEdge in allEdges)
        {
            int parentIndex = GetComponent(directedEdge.parent);
            int childIndex = GetComponent(directedEdge.child);
            DirectedEdge newDirectedEdge = directedEdge; // copy struct
            newDirectedEdge.seperatorEdge = false;       // irrelevant here
            if (parentIndex != childIndex)
            {
                newDirectedEdge.treeEdge = true;
                nextComponent.Add(childIndex, parentIndex);
                edges.Add(newDirectedEdge.edge);
            } else
            {
                newDirectedEdge.treeEdge = false;
            }
            newDirectedEdges.Add(newDirectedEdge);
        }

        allEdges = newDirectedEdges;

        // now that we have all the edges in the spanning tree, we need to choose
        // their directions to create a directed tree.
        Subgraph<GraphNode, GraphEdge> subgraph = 
            new Subgraph<GraphNode, GraphEdge>(this, edge => edges.Contains(edge));

        Dictionary<GraphNode, int> nodeIndices =
            new Dictionary<GraphNode, int> { { root, 0 } };
        int nodeIndex = 1;
        void TrackNodeDiscovery(
            GraphNode fromNode, int fromPosition, int fromSmallestVisited,
            GraphNode toNode, int toPosition, int toSmallestVisited,
            GraphEdge edge, EdgeTravelType travelType)
        {
            if (travelType == EdgeTravelType.FIRST_DISCOVER)
            {
                nodeIndices.Add(toNode, nodeIndex);
                nodeIndex++;
            }
        }


        subgraph.DFS(root, TrackNodeDiscovery);

        newDirectedEdges = new List<DirectedEdge>();
        foreach (var directedEdge in allEdges)
        {
            int parentIndex = nodeIndices[directedEdge.parent];
            int childIndex = nodeIndices[directedEdge.child];
            if (childIndex < parentIndex)
            {
                DirectedEdge newDirectedEdge = directedEdge;
                newDirectedEdge.parent = directedEdge.child;
                newDirectedEdge.child = directedEdge.parent;
                newDirectedEdges.Add(newDirectedEdge);
            } else
            {
                newDirectedEdges.Add(directedEdge);
            }
        }

        return newDirectedEdges;

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

        public override string ToString()
        {
            if (parent is Object && child is Object)
            {
                string treeString = treeEdge ? "tree" : "" ;
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


