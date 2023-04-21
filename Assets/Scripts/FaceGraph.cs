using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using DirectedEdge = GraphStructure<Face, Axis>.DirectedEdge;

public class FaceGraph : MonoBehaviour
{    
    [SerializeField] private int numberOfFaces = 0;
    private Face rootFace; // TODO change to private
    // TODO : add list of edges
    public bool runButton = false;

    class FaceGraphStructure : GraphStructure<Face, Axis>
    {
        private bool randomizeEdges;

        public FaceGraphStructure(bool randomizeEdges = true)
        {
            this.randomizeEdges = randomizeEdges;
        }

        public override IEnumerable<Axis> GetEdgesOf(Face node)
        {
            if (randomizeEdges)
                return MathTools.RandomList(node.edges);
            else
                return node.edges;
        }

        public override Face GetNode1Of(Axis edge)
        {
            return edge.GetFace1();
        }

        public override Face GetNode2Of(Axis edge)
        {
            return edge.GetFace2();
        }

        public override Face GetOtherNodeOf(Axis edge, Face excludeNode)
        {
            return edge.GetOtherFace(excludeNode);
        }
    }

    private FaceGraphStructure dfsGraph;

    private void Awake()
    {
        dfsGraph = new FaceGraphStructure();
    }


    private bool IsFullTree(IEnumerable<DirectedEdge> directedEdges)
    {
        int treeEdges = directedEdges.Where(directedEdge => directedEdge.treeEdge).Count();
        return treeEdges + 1 == numberOfFaces;
    }

    private void SetParentFromDirectedEdges(IEnumerable<DirectedEdge> directedEdges)
    {
        foreach (DirectedEdge directedEdge in directedEdges)
        {
            directedEdge.edge.transform.parent = directedEdge.parent.transform;

            if (directedEdge.treeEdge && directedEdge.child != null)
                directedEdge.child.transform.parent = directedEdge.edge.transform;
        }
    }


    private IEnumerable<DirectedEdge> CreateInitialGraph()
    {
        if (rootFace == null)
            return null;

        // Run a DFS algorithm on the faces as nodes, where we start at the root and two faces are connected in the graph
        // when there is an edge connecting them.
        // The returned list of edges contains the information of whether the edge is part of a cycle or not.
        List<DirectedEdge> directedEdges = dfsGraph.GetDirectedEdgesDFS(rootFace);

        // check if the graph is connected (tree edges = #nodes - 1)
        if (!IsFullTree(directedEdges))
        {
            Debug.Log("The graph was disconnected");
            return null;
        }

        // Move all the faces to be children of the root face.
        foreach (DirectedEdge directedEdge in directedEdges)
        {
            // Just to make sure that we don't accidently create loops
            directedEdge.edge.transform.parent = rootFace.transform;
            if (directedEdge.child != rootFace && directedEdge.child != null)
                directedEdge.child.transform.parent = rootFace.transform;
        }

        SetParentFromDirectedEdges(directedEdges);

        return directedEdges;
    }

    public void CreateRandomGraph()
    {
        IEnumerable<DirectedEdge> directedEdges = CreateInitialGraph();

        foreach (DirectedEdge directedEdge in directedEdges)
        {
            if (directedEdge.treeEdge)
            {
                directedEdge.edge.SetAngle(directedEdge.parent, 180);
            } else 
            {
                directedEdge.edge.Disconnect();
            }
        }
    }

    public void CreateTransformGraph()
    {
        IEnumerable<DirectedEdge> directedEdges = CreateInitialGraph();
        if (directedEdges == null)
            return;

        Dictionary<Face, int> componentOf = dfsGraph.GetStronglyConnectedComponents(rootFace);
        foreach (DirectedEdge directedEdge in directedEdges)
        {
            //directedEdge.edge.transform.parent = directedEdge.parent.transform;
            if (directedEdge.edge.IsSelected())
            {
                Face face1 = directedEdge.edge.GetFace1();
                Face face2 = directedEdge.edge.GetFace2();
                if (face1 == null || face2 == null || componentOf[face1] != componentOf[face2])
                {
                    directedEdge.edge.Disconnect();
                    directedEdge.edge.GetComponentInChildren<MouseTarget>().SetSelectable(false); // TODO : refactor this part
                }
                continue;
            }
            //if (directedEdge.child != null)
            //    directedEdge.child.transform.parent = directedEdge.edge.transform;
            if (directedEdge.seperatorEdge)
            {
                directedEdge.edge.StartRotating(directedEdge.parent);
            }
        }
    }

    public List<Vector2Int> sampleGraph;

    private void OnValidate()
    {
        if (runButton)
        {
            CreateTransformGraph();

            (int, int)[] edges = new (int x, int y)[sampleGraph.Count];
            int index = 0;
            int n = 0;
            foreach (var v in sampleGraph)
            {
                edges[index] = (v.x, v.y);
                index++;
                n = Mathf.Max(n, v.x + 1);
                n = Mathf.Max(n, v.y + 1);
            }
            SimpleGraph graph = new SimpleGraph();
            SimpleGraphNode root = graph.CreateGraph(n, edges);
            graph.GetDirectedEdgesDFS(root);
        }
    }

    public void SetPolyhedron(Face rootFace, int numberOfFaces)
    {
        this.rootFace = rootFace;
        this.numberOfFaces = numberOfFaces;
    }

    public bool CompareTo(FaceGraph other)
    {
        List<DirectedEdge> directedEdges = dfsGraph.GetDirectedEdgesDFS(rootFace);
        List<DirectedEdge> otherDirectedEdges = dfsGraph.GetDirectedEdgesDFS(other.rootFace);

        List<(int, int)> numberedEdges = (
            from directedEdge in directedEdges
            where !directedEdge.edge.IsSelected()
            select directedEdge.edge.AsTuple()).ToList();
        numberedEdges.Sort();

        List<(int, int)> otherNumberedEdges = (
            from directedEdge in otherDirectedEdges
            where !directedEdge.edge.IsSelected()
            select directedEdge.edge.AsTuple()).ToList();
        otherNumberedEdges.Sort();

        return numberedEdges.SequenceEqual(otherNumberedEdges);
    }
}

