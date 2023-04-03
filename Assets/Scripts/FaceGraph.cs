using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class FaceGraph : MonoBehaviour
{

    [SerializeField] private Face rootFace; // TODO change to private
    [SerializeField] private int numberOfFaces = 0;
    public bool runButton = false;

    class DFSFaceGraph : GraphStructure<Face, Axis>
    {
        public override IEnumerable<Axis> GetEdgesOf(Face node)
        {
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

    private DFSFaceGraph dfsGraph;

    private void Awake()
    {
        dfsGraph = new DFSFaceGraph();
    }

    private void Start()
    {
        PlayerInput input = new PlayerInput();
        input.Player.Enable();
        input.Player.Graph.performed += CreateGraph;
    }

    private void CreateGraph(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        CreateTransformGraph();
    }

    public void CreateTransformGraph()
    {
        Face root = rootFace;
        if (root == null)
            return;

        Dictionary<Face, int> componentOf = dfsGraph.GetStronglyConnectedComponents(root);


        // Run a DFS algorithm on the faces as nodes, where we start at the root and two faces are connected in the graph
        // when there is an edge connecting them.
        // The returned list of edges contains the information of whether the edge is part of a cycle or not.
        List<GraphStructure<Face, Axis>.DirectedEdge> directedEdges = dfsGraph.GetDirectedEdgesDFS(root);

        // check if the graph is connected (tree edges = #nodes - 1)
        // and move all the faces to be children of the root face.
        int treeEdges = 0;
        foreach (GraphStructure<Face, Axis>.DirectedEdge directedEdge in directedEdges)
        {
            // Just to make sure that we don't accidently create loops
            directedEdge.edge.transform.parent = root.transform;
            if (directedEdge.treeEdge)
                treeEdges += 1;
            if (directedEdge.child!=null) 
                directedEdge.child.transform.parent = root.transform;
        }

        if (treeEdges + 1 < numberOfFaces)
        {
            Debug.Log("The graph was disconnected");
            return;
        }

        
        foreach (GraphStructure<Face, Axis>.DirectedEdge directedEdge in directedEdges)
        {
            directedEdge.edge.transform.parent = directedEdge.parent.transform;
            if (directedEdge.edge.IsSelected())
            {
                Face face1 = directedEdge.edge.GetFace1();
                Face face2 = directedEdge.edge.GetFace2();
                if (face1 == null || face2 == null || componentOf[face1] !=componentOf[face2])
                    directedEdge.edge.Disconnect();
            }
            if (directedEdge.child != null)
                directedEdge.child.transform.parent = directedEdge.edge.transform;
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

}

