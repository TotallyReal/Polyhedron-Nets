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
    [SerializeField] private Axis axisPrefab;
    [SerializeField] private Material edgeMaterial;
    [SerializeField] private Material rootMaterial;

    [Header("MISC")]
    [SerializeField] private float numberLabelDistance = 0.003f;
    [SerializeField] private RotatorArrow rotatorArrow;
    [SerializeField] private NumberedCanvas numberedCanvasPrefab;
    [SerializeField] private bool touchMe;

    private void OnValidate()
    {
        if (touchMe)
        {
            /*IsometryGroup group = new IsometryGroup();
            IsoElement g = group.AddElement(Matrix.Rotation3(1, 1, 1, 2 * Mathf.PI / 3));
            g.GetPresentation(out Vector3 v, out bool flip);*/
        }
    }

    private GameObject polyhedron = null;
    private VisualPolyhedron visualPolyhedron;
    private FaceGraph graph;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreatePolyhedron();
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
        AbstractGroupPolyhedron abstractPolyhedron = AbstractGroupPolyhedron.Dodecahedron(10);
        //AbstractPolyhedron abstractPolyhedron = AbstractGroupPolyhedron.Isocahedron(10); // = AbstractPolyhedron.CreateCube(5);

        FaceMesh rootFace = CreatePolyhedron(abstractPolyhedron);

        graph = GetComponent<FaceGraph>();
        graph.SetPolyhedron(rootFace, abstractPolyhedron.GetFaces().Count);
        rootFace.SetMaterial(rootMaterial);

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

    /// <summary>
    /// Create a new face from the given vertices.
    /// </summary>
    /// <param name="faceVertices"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private FaceMesh CreateFace(Vector3[] faceVertices, int index)
    {
        FaceMesh face = Instantiate<FaceMesh>(facePrefab, polyhedron.transform);
        face.name = $"Face {index}";
        face.CreateMesh(faceVertices);

        NumberedCanvas numberedCanvas1 = Instantiate<NumberedCanvas>(numberedCanvasPrefab, face.transform);
        numberedCanvas1.transform.position = face.Center + numberLabelDistance * face.LocalNormal.normalized;
        numberedCanvas1.transform.forward = -face.LocalNormal;
        numberedCanvas1.SetNumber(index);

        NumberedCanvas numberedCanvas2 = Instantiate<NumberedCanvas>(numberedCanvasPrefab, face.transform);
        numberedCanvas2.transform.position = face.Center - numberLabelDistance * face.LocalNormal.normalized;
        numberedCanvas2.transform.forward = face.LocalNormal;
        numberedCanvas2.SetNumber(index);

        return face;
    }

    public FaceMesh CreatePolyhedron(AbstractGroupPolyhedron absPolyhedron)
    {
        if (polyhedron != null)
        {
            Destroy(polyhedron);
        }
        polyhedron = new GameObject("Polyhedron");
        visualPolyhedron = polyhedron.AddComponent<VisualPolyhedron>();
        visualPolyhedron.SetAbstractPolyhedron(absPolyhedron);
        rotatorArrow.SetObjectToRotate(polyhedron.transform);

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
                edge = Instantiate<Axis>(axisPrefab, polyhedron.transform);
                edge.SetVisual(v1, v2);

                edgesDict.Add((index1, index2), edge);

                visualPolyhedron.AddEdge(edge);

                //edge.transform.parent = polyhedron.transform;
            }
            return edge;
        }

        List<FaceMesh> faces = new List<FaceMesh>();
        FaceMesh rootFace = null;
        int faceIndex = 0;

        Vector3[] vertices = absPolyhedron.GetVertices();
        Vector3 rotationAxis = Vector3.up;
        float rotationAngle = 0;

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
            FaceMesh face = CreateFace(faceVertices, faceIndex++);
            visualPolyhedron.AddFace(face);
            if (rootFace == null) { 
                // make sure that the root face is facing downwards.
                rootFace = face;
                Vector3 normalDir = rootFace.Normal();
                rotationAxis = Vector3.Cross(normalDir, polyhedron.transform.up);
                rotationAngle = 180 - Vector3.Angle(normalDir, polyhedron.transform.up);
            }
            faces.Add(face);

            // find\create the edges and connect them to the face
            foreach ((int i, int j) in abstractFace.EdgesIndices())
            {
                Axis edge = TryGetEdge(i, j, vertices[i], vertices[j]);
                if (edge.face1 == null)
                    edge.face1 = face;
                else
                {
                    edge.face2 = face;
                    edge.name = $"Edge [{edge.face1.name}, {edge.face2.name}]";
                }
                face.edges.Add(edge);
            }
        }

        // rotate the polyhedron, so that the root face will be at the bottom.
        foreach (Axis axis in edgesDict.Values)
        {
            axis.transform.RotateAround(Vector3.zero, rotationAxis, -rotationAngle);
        }
        foreach (Face face in faces)
        {
            face.transform.Rotate(rotationAxis, -rotationAngle);
        }

        //Vector3 normalDir = rootFace.Normal();
        //Vector3 rotationAxis = Vector3.Cross(normalDir, polyhedron.transform.up);
        //float angle = 180 - Vector3.Angle(normalDir, polyhedron.transform.up);
        //polyhedron.transform.Rotate(rotationAxis, -angle);

        visualPolyhedron.SetFaceMaterial(faceMaterial);
        visualPolyhedron.SetEdgeMaterial(edgeMaterial);
        ChooseTexture.Instance.SetVisualPolyhedron(visualPolyhedron);

        return rootFace;

    }

    public bool NearAxis(Vector3 v, float error)
    {
        if (visualPolyhedron == null)
            return false;
        return visualPolyhedron.NearRotationAxis(v, error);
    }
}

