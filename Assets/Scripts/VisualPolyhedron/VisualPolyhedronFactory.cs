using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.ProBuilder;

/// <summary>
/// Used to create visual polyhedrons out of abstract ones
/// </summary>
public class VisualPolyhedronFactory : MonoBehaviour
{
    public static VisualPolyhedronFactory Instance { get; private set; } // TODO should I keep this static variable?

    [Header("Polyhedron components")]
    [SerializeField] private FaceMesh facePrefab;
    [SerializeField] private Material faceMaterial;
    [SerializeField] private Material rootMaterial;
    [Space(10)]
    [SerializeField] private Axis axisPrefab;
    [SerializeField] private Material edgeMaterial;
    [Space(10)]
    [SerializeField] private float numberLabelDistance = 0.003f;
    [SerializeField] private NumberedCanvas numberedCanvasPrefab;

    [Header("MISC")]
    [ContextMenuItem("Variable menu", "VariableAction")]
    [SerializeField] private RotatorArrow rotatorArrow;
    [SerializeField] private Transform floor;

    [ContextMenu("this is a context menu")]
    private void MenuAction()
    {
        Debug.Log("Context menu");
    }
    private void VariableAction()
    {
        Debug.Log("Variable menu");
    }

    private VisualPolyhedron visualPolyhedron = null;
    private VisualPolyhedron shadowVisualPolyhedron = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreatePolyhedron();

        PlayerInput input = new PlayerInput();
        input.Player.Enable();
        input.Player.Graph.performed += CreateGraph;
    }
    private void CreateGraph(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (visualPolyhedron!=null)
            visualPolyhedron.GetComponent<FaceGraph>().CreateTransformGraph();
    }

    public void CreatePolyhedron()
    {

        /*abstractPolyhedron = new CubicPolyhedron(new Vector3Int[] {
            new Vector3Int(0,-1,0),
            new Vector3Int(0,0,0),
            new Vector3Int(0,1,0),
            new Vector3Int(1,0,0),
            new Vector3Int(-1,0,0),
        }, new Vector3Int(0, -2, 0), new Vector3Int(0, -1, 0), 5);*/
        //AbstractPolyhedron abstractPolyhedron = AbstractPolyhedron.CreateCube(5);
        //AbstractGroupPolyhedron abstractPolyhedron = AbstractGroupPolyhedron.Dodecahedron(10);
        AbstractGroupPolyhedron abstractPolyhedron = AbstractGroupPolyhedron.Isocahedron(10);
        //AbstractGroupPolyhedron abstractPolyhedron = AbstractGroupPolyhedron.Cube(10);

        if (visualPolyhedron != null)
        {
            Destroy(visualPolyhedron.gameObject);
            Destroy(shadowVisualPolyhedron.gameObject);
        }

        visualPolyhedron = CreatePolyhedron(abstractPolyhedron).GetComponent<VisualPolyhedron>();
        visualPolyhedron.gameObject.name = "visual polyhedron";
        ChooseTexture.Instance.SetVisualPolyhedron(visualPolyhedron); // TODO: remove

        shadowVisualPolyhedron = CreatePolyhedron(abstractPolyhedron).GetComponent<VisualPolyhedron>();
        shadowVisualPolyhedron.gameObject.name = "Shadow polyhedron";
        shadowVisualPolyhedron.SetFaceMaterial(rootMaterial);

        FaceMesh shadowRoot = shadowVisualPolyhedron.RootFace;
        
        float dy = floor.position.y - shadowRoot.transform.TransformPoint(shadowRoot.Center).y;
        dy += 0.5f;
        shadowVisualPolyhedron.transform.position += new Vector3(0, dy, 0);


        FaceGraph faceGraph = shadowVisualPolyhedron.GetComponent<FaceGraph>();
        faceGraph.CreateRandomGraph();
    }

    /*public FaceMesh CreatePolyhedron(Vector3[] vertices, List<int[]> facesList) // TODO restore this method
    {
        AbstractPolyhedron absPolyhedron = new AbstractPolyhedron();
        foreach (Vector3 v in vertices)
        {
            absPolyhedron.AddVertex(v);
        }
        foreach (var face in facesList)
        {
            absPolyhedron.AddFace(face);
        }

        return CreatePolyhedron(absPolyhedron);
    }*/

    #region -------------------- edges and faces creation --------------------

    /// <summary>
    /// Creates a new face from the given vertices, and add a numbering on it.
    /// </summary>
    /// <param name="faceVertices"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private FaceMesh CreateFace(
        VisualPolyhedron visualPolyhedron, Vector3[] faceVertices, int index, bool addNumbering = true)
    {
        FaceMesh face = Instantiate(facePrefab, visualPolyhedron.transform);
        face.ID = index;
        face.name = $"Face {index}";
        face.CreateMesh(faceVertices);

        if (addNumbering)
        {
            NumberedCanvas numberedCanvas1 = Instantiate<NumberedCanvas>(numberedCanvasPrefab, face.transform);
            numberedCanvas1.transform.position = face.Center + numberLabelDistance * face.LocalNormal.normalized;
            numberedCanvas1.transform.forward = -face.LocalNormal;
            numberedCanvas1.SetNumber(index);

            NumberedCanvas numberedCanvas2 = Instantiate<NumberedCanvas>(numberedCanvasPrefab, face.transform);
            numberedCanvas2.transform.position = face.Center - numberLabelDistance * face.LocalNormal.normalized;
            numberedCanvas2.transform.forward = face.LocalNormal;
            numberedCanvas2.SetNumber(index);
        }

        return face;
    }

    private VisualPolyhedron CreateFacesAndEdges(AbstractGroupPolyhedron absPolyhedron, VisualPolyhedron visualPolyhedron)
    {
        visualPolyhedron.SetAbstractPolyhedron(absPolyhedron);
        Dictionary<(int, int), Axis> edgesDict = new Dictionary<(int, int), Axis>();

        // Try to get the edge vetween the vertices with the given indices, if exists.
        // If not, create it and return it.
        Axis TryGetEdge(int index1, int index2, Vector3 v1, Vector3 v2)
        {
            Axis edge;
            if (!edgesDict.TryGetValue((index1, index2), out edge) &&
                !edgesDict.TryGetValue((index2, index1), out edge))
            {
                // edge does not exist - create it
                edge = Instantiate<Axis>(axisPrefab, visualPolyhedron.transform);
                edge.SetVisual(v1, v2);

                edgesDict.Add((index1, index2), edge);

                visualPolyhedron.AddEdge(edge);
            }
            return edge;
        }

        //FaceMesh rootFace = null;
        int faceIndex = 0;

        Vector3[] vertices = absPolyhedron.GetVertices();
        //Vector3 rotationAxis = Vector3.up;
        //float rotationAngle = 0;

        // Create the faces
        foreach (var abstractFace in absPolyhedron.GetAbstractFaces())
        {
            // Collect the positions of the face's vertices
            Vector3[] faceVertices = new Vector3[abstractFace.VertexCount()];
            int index = 0;
            foreach (var vertexIndex in abstractFace.VertexIndices())
            {
                faceVertices[index] = vertices[vertexIndex];
                index++;
            }

            // Create the face's GameObject
            FaceMesh face = CreateFace(visualPolyhedron, faceVertices, faceIndex++);
            visualPolyhedron.AddFace(face);
            if (visualPolyhedron.RootFace == null)
            {
                visualPolyhedron.RootFace = face;
            }

            // find\create the edges and connect them to the face
            foreach ((int i, int j) in abstractFace.EdgesIndices())
            {
                Axis edge = TryGetEdge(i, j, vertices[i], vertices[j]);
                if (edge.GetFace1() == null)
                    edge.SetFace1(face);
                else
                {
                    edge.SetFace2(face);
                    edge.name = $"Edge [{edge.GetFace1().name}, {edge.GetFace2().name}]";
                }
            }
        }

        return visualPolyhedron;
    }

    #endregion

    /// <summary>
    /// Rotates the polyhedron so that the root mesh will be facing downwards
    /// </summary>
    /// <param name="components"></param>
    /// <returns></returns>
    private void RotatePolyhedron(VisualPolyhedron visualPolyhedron)
    {
        Vector3 normalDir = visualPolyhedron.RootFace.Normal();
        Vector3 rotationAxis = Vector3.Cross(normalDir, visualPolyhedron.transform.up);
        float rotationAngle = 180 - Vector3.Angle(normalDir, visualPolyhedron.transform.up);

        // rotate the polyhedron, so that the root face will be at the bottom.
        foreach (Axis axis in visualPolyhedron.GetEdges())
        {
            axis.transform.RotateAround(Vector3.zero, rotationAxis, -rotationAngle);
        }
        foreach (Face face in visualPolyhedron.GetFaces())
        {
            face.transform.RotateAround(Vector3.zero, rotationAxis, -rotationAngle);
        }
    }

    public GameObject CreatePolyhedron(AbstractGroupPolyhedron absPolyhedron)
    {
        // ------------------- create polyhedron object -------------------
        GameObject polyhedron = new GameObject("Polyhedron");

        VisualPolyhedron visualPolyhedron = polyhedron.AddComponent<VisualPolyhedron>();
        CreateFacesAndEdges(absPolyhedron, visualPolyhedron);
        RotatePolyhedron(visualPolyhedron);  // so that the root face will face downwards

        visualPolyhedron.SetFaceMaterial(faceMaterial);
        visualPolyhedron.SetEdgeMaterial(edgeMaterial);

        // ------------------- face graph -------------------
        FaceGraph graph = polyhedron.AddComponent<FaceGraph>();
        graph.SetPolyhedron(visualPolyhedron.RootFace, absPolyhedron.GetFaces().Count);
        visualPolyhedron.RootFace.SetMaterial(rootMaterial);

        // ------------------- rotator arrow -------------------
        rotatorArrow.SetObjectToRotate(polyhedron.transform); // TODO: remove 

        return polyhedron;
    }

    public VisualPolyhedron GetVisualPolyhedron()
    {
        return visualPolyhedron;
    }

    public void CompareUnfolding()
    {
        FaceGraph faceGraph = visualPolyhedron.GetComponent<FaceGraph>();
        FaceGraph shadowFaceGraph = shadowVisualPolyhedron.GetComponent<FaceGraph>();
        Debug.Log($"Graph comparison {faceGraph.CompareTo(shadowFaceGraph)}");
    }

    internal void CreateTransformGraph()
    {
        visualPolyhedron.GetComponent<FaceGraph>().CreateTransformGraph();
    }
}

