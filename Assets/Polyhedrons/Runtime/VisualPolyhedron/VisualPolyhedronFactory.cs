using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Used to create visual polyhedrons out of abstract ones
/// </summary>
[Serializable]
public class VisualPolyhedronFactory : MonoBehaviour
{
    //public static VisualPolyhedronFactory Instance { get; private set; } // TODO should I keep this static variable?

    public enum PolyhedronShape
    {
        CUBE,
        OCTAHEDRON,
        CUBE_COLLECTION,
        DODECAHEDRON,
        ICOSAHEDRON,
        TETRAHEDRON
    }

    [Header("Polyhedron components")]
    [SerializeField] private string polyhedronName;
    [SerializeField] private VisualPolyhedron VisualPolyhedronPrefab;
    [SerializeField] private PolyhedronShape defaultShape = PolyhedronShape.CUBE;
    [SerializeField] private VisualPolyhedronProperties polyhedronProperties;
    [SerializeField] private bool createPolyhedronOnStartup = true;
    [Header("Faces")]
    [SerializeField] private FaceMesh facePrefab;
    [Header("Edges")]
    [SerializeField] private PolyhedronEdge axisPrefab;
    [Header("Vertices")]
    [SerializeField] private Transform vertexPrefab;
    [Header("Numbers")]
    [SerializeField] private NumberedCanvas numberedCanvasPrefab;


    private void Start()
    {
        if (createPolyhedronOnStartup)
            CreatePolyhedron();
    }

    private List<Vector3Int> cubePositions = new List<Vector3Int>(){
            new Vector3Int(0,-1,0),
            new Vector3Int(0,0,0),
            //new Vector3Int(0,1,0),
            new Vector3Int(1,0,0),
            new Vector3Int(-1,0,0)
        };

    public void AddCube(FaceMesh face)
    {
        Vector3Int existingPosition = Vector3Int.CeilToInt((-face.Normal() + face.GetGlobalCenter() / polyhedronProperties.faceRadius) / 2);
        if (!cubePositions.Contains(existingPosition))
            return;
        Vector3Int position = Vector3Int.CeilToInt((face.Normal() + face.GetGlobalCenter() / polyhedronProperties.faceRadius) / 2);
        cubePositions.Add(Vector3Int.CeilToInt(position));
        // TODO : what happens when adding cubes on the root face?
        CreatePolyhedron();
    }

    private AbstractPolyhedron GetDefaultPolyhedron()
    {
        float faceRadius = polyhedronProperties.faceRadius;
        switch (defaultShape)
        {
            case PolyhedronShape.CUBE:
                return AbstractGroupPolyhedron.Cube(faceRadius);
            case PolyhedronShape.OCTAHEDRON:
                return AbstractGroupPolyhedron.Octahedron(faceRadius);
            case PolyhedronShape.TETRAHEDRON:
                return AbstractGroupPolyhedron.Tetrahedron(faceRadius);
            case PolyhedronShape.DODECAHEDRON:
                return AbstractGroupPolyhedron.Dodecahedron(faceRadius);                
            case PolyhedronShape.ICOSAHEDRON:
                return AbstractGroupPolyhedron.Isocahedron(faceRadius);
            case PolyhedronShape.CUBE_COLLECTION:
                return new CubicPolyhedron(
                    cubePositions.ToArray(),
                    new Vector3Int(0, -2, 0), new Vector3Int(0, -1, 0), faceRadius);
            default:
                return AbstractGroupPolyhedron.Cube(faceRadius);

        }
    }


    public event EventHandler<AbstractGroupPolyhedron> OnPolyhedroneGenerated;

    private void SetDefaultFaceGraph(VisualPolyhedron visualPolyhedron)
    {
        foreach (PolyhedronEdge edge in visualPolyhedron.GetEdges())
        {
            (int, int) value = edge.AsTuple();
            if (value.Item1 == 0 || value.Item2 == 0 || value.Item1 == 11 || value.Item2 == 11)
                continue;
            if (value.Item1 == 1 && value.Item2 == 7)
                continue;
            MouseTarget mouseTarget = edge.GetMouseTarget();
            if (mouseTarget == null)
            {
                Debug.LogError("shouldn't be here");
            }
            else
                mouseTarget.SelectTarget();
        }
    }

    #region ------------------------- Polyhedron creation -------------------------

    [Tooltip("The first extra step called after generation of the polyhedron")]
    [SerializeField] private VisualPolyhedronFactoryStep firstStep;
    [Tooltip("The rest of the steps. Note that order of execution is not guaranteed")]
    public UnityEvent<VisualPolyhedron> extraCreationSteps;

    /// <summary>
    /// Creates a new polyhedron, and destorys the previous one if exists
    /// </summary>
    public void CreatePolyhedron()
    {
        DestroyPolyhedron();
        CreateNewPolyhedron();
    }


    public void DestroyPolyhedron()
    {
        if (visualPolyhedron != null)
        {
            if (Application.isPlaying)
            {
                Destroy(visualPolyhedron.gameObject);
            }
            else
            {
                DestroyImmediate(visualPolyhedron.gameObject);
            }
        }

    }

    // TODO: Adding serializeField, so I can generate it in editor.
    //       Need to refactor the whole code.
    [Header("Serialized polyhedron for editor")]
    private AbstractPolyhedron abstractPolyhedron;
    [SerializeField] private VisualPolyhedron visualPolyhedron = null;

    /// <summary>
    /// Creates a new polyhedron, without destroying existing polyhedrons
    /// </summary>
    public void CreateNewPolyhedron() { 

        abstractPolyhedron = GetDefaultPolyhedron();
        visualPolyhedron = CreatePolyhedron(abstractPolyhedron).GetComponent<VisualPolyhedron>();
        visualPolyhedron.gameObject.name = polyhedronName != null ? polyhedronName : "Polyhedron";
        // TODO: change parent to be the rootPosition
        visualPolyhedron.transform.SetParent(transform);

        firstStep.FactoryStep(visualPolyhedron);

        extraCreationSteps?.Invoke(visualPolyhedron);

        if (abstractPolyhedron is AbstractGroupPolyhedron)
        {
            OnPolyhedroneGenerated?.Invoke(this, (AbstractGroupPolyhedron)abstractPolyhedron);
        }
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

    #region -------------------- polyhedron object creation creation --------------------

    public void AddVertices()
    {
        if (visualPolyhedron == null || vertexPrefab == null)
            return;
        visualPolyhedron.AddVertices(vertexPrefab);
    }

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
            numberedCanvas1.SetPosition(
                center: face.Center + polyhedronProperties.numberLabelDistance * face.LocalNormal.normalized,
                direction: -face.LocalNormal);
            numberedCanvas1.SetRadius(polyhedronProperties.numberRadius);
            numberedCanvas1.SetNumber(index);

            NumberedCanvas numberedCanvas2 = Instantiate<NumberedCanvas>(numberedCanvasPrefab, face.transform);
            numberedCanvas2.SetPosition(
                center: face.Center - polyhedronProperties.numberLabelDistance * face.LocalNormal.normalized,
                direction: face.LocalNormal);
            numberedCanvas2.SetRadius(polyhedronProperties.numberRadius);
            numberedCanvas2.SetNumber(index);
        }

        return face;
    }

    private VisualPolyhedron CreateFacesAndEdges(AbstractPolyhedron absPolyhedron, VisualPolyhedron visualPolyhedron)
    {
        visualPolyhedron.SetAbstractPolyhedron(absPolyhedron);
        Dictionary<(int, int), PolyhedronEdge> edgesDict = new Dictionary<(int, int), PolyhedronEdge>();

        // Try to get the edge vetween the vertices with the given indices, if exists.
        // If not, create it and return it.
        PolyhedronEdge TryGetEdge(int index1, int index2, Vector3 v1, Vector3 v2)
        {
            PolyhedronEdge edge;
            if (!edgesDict.TryGetValue((index1, index2), out edge) &&
                !edgesDict.TryGetValue((index2, index1), out edge))
            {
                // edge does not exist - create it
                edge = Instantiate<PolyhedronEdge>(axisPrefab, visualPolyhedron.transform);
                edge.SetVisual(v1, v2, 0.1f);
                //edge.SetVisual(v1, v2, polyhedronProperties.edgeRadius);

                edgesDict.Add((index1, index2), edge);

                visualPolyhedron.AddEdge(edge);
            }
            //edge.SetVisible(polyhedronProperties.showEdges);
            return edge;
        }

        //FaceMesh rootFace = null;
        int faceIndex = 0;

        Vector3[] vertices = absPolyhedron.GetVertices();

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
            FaceMesh face = CreateFace(visualPolyhedron, faceVertices, faceIndex++, polyhedronProperties.showNumbers);
            visualPolyhedron.AddFace(face);
            if (visualPolyhedron.RootFace == null)
            {
                visualPolyhedron.RootFace = face;
            }

            // find\create the edges and connect them to the face
            foreach ((int i, int j) in abstractFace.EdgesIndices())
            {
                PolyhedronEdge edge = TryGetEdge(i, j, vertices[i], vertices[j]);
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

    public GameObject CreatePolyhedron(AbstractPolyhedron absPolyhedron)
    {
        // ------------------- create polyhedron object -------------------
        VisualPolyhedron visualPolyhedron = Instantiate<VisualPolyhedron>(VisualPolyhedronPrefab);
        visualPolyhedron.enabled = true;
        Debug.Log("Instantiated visual polyhedron");

        CreateFacesAndEdges(absPolyhedron, visualPolyhedron);

        // ------------------- face graph -------------------
        FaceGraph graph = visualPolyhedron.GetFaceGraph();
        graph.SetPolyhedron(visualPolyhedron.RootFace, absPolyhedron.GetFaces().Count);

        return visualPolyhedron.gameObject;
    }

    #endregion


    #region -------------------- extra creation steps --------------------


    #endregion


    #endregion
    public VisualPolyhedron GetVisualPolyhedron()
    {
        return visualPolyhedron;
    }

    public AbstractPolyhedron GetAbstractPolyhedron()
    {
        if (visualPolyhedron != null)
        {
            return visualPolyhedron.GetAbstractPolyhedron();
        }
        return null;
    }

    public void CompareUnfolding()
    {
        FaceGraph faceGraph = visualPolyhedron.GetComponent<FaceGraph>();
        //FaceGraph shadowFaceGraph = shadowVisualPolyhedron.GetComponent<FaceGraph>();
        //Debug.Log($"Graph comparison {faceGraph.CompareTo(shadowFaceGraph)}");
    }

    public void CreateTransformGraph()
    {
        visualPolyhedron.GetComponent<FaceGraph>().CreateTransformGraph();
    }
}

