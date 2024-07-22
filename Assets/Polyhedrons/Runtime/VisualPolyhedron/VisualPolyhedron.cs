using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: This is for generating polyhedrons in editor
//       Find a better way of doing it
[ExecuteInEditMode]
public class VisualPolyhedron : MonoBehaviour
{

    [SerializeField] private VisualPolyhedronProperties visualProperties;
    [SerializeField] private Material faceMaterial;
    [SerializeField] private Material edgeMaterial;

    private List<PolyhedronEdge> edges;
    private List<FaceMesh> faces;
    public FaceMesh RootFace { get; set; }
    private FaceGraph faceGraph;

    private void Awake()
    {
        edges = new List<PolyhedronEdge>();
        faces = new List<FaceMesh>();

        faceGraph = GetComponent<FaceGraph>();
    }

    private void OnValidate()
    {
        if (faces != null && faceMaterial != null)
        {
            SetFaceMaterial(faceMaterial);
        }
        if (edges != null && edgeMaterial != null)
        {
            SetEdgeMaterial(edgeMaterial);
        }
    }

    private AbstractPolyhedron absPolyhedron;

    public void SetAbstractPolyhedron(AbstractPolyhedron absPolyhedron)
    {
        this.absPolyhedron = absPolyhedron;
    }
    public AbstractPolyhedron GetAbstractPolyhedron()
    {
        return absPolyhedron; // TODO: Either return a copy, or remove this method
    }

    public FaceGraph GetFaceGraph()
    {
        return faceGraph;
    }

    public void SetVisualProperties(VisualPolyhedronProperties visualProperties) {
        if (visualProperties == null)
            return;

        this.visualProperties = visualProperties;

        // faces :
        SetFaceMaterial(visualProperties.faceMaterial);
        RootFace.SetMaterial(visualProperties.rootMaterial);

        // edges :
        SetEdgeMaterial(visualProperties.edgeMaterial);
        SetSelectedEdgeMaterial(visualProperties.selectedEdgeMaterial);
        foreach (var edge in edges)
        {
            edge.SetRadius(visualProperties.edgeRadius);
            edge.SetVisible(visualProperties.showEdges);
        }
    }

    #region --------------------- faces ---------------------
    public void AddFace(FaceMesh face)
    {
        faces.Add(face);
    }

    public void SetFaceMaterial(Material faceMaterial)
    {
        this.faceMaterial = faceMaterial;
        foreach (var face in faces)
        {
            face.SetMaterial(faceMaterial);
        }
    }

    public IEnumerable<FaceMesh> GetFaces()
    {
        return faces;
    }

    #endregion

    #region --------------------- edges ---------------------
    public void AddEdge(PolyhedronEdge axis)
    {
        edges.Add(axis);
    }

    public void SetEdgeMaterial(Material edgeMaterial)
    {
        this.edgeMaterial = edgeMaterial;
        foreach (var edge in edges)
        {
            edge.SetMaterial(edgeMaterial);
        }
    }

    public void SetSelectedEdgeMaterial(Material edgeMaterial)
    {
        foreach (var edge in edges)
        {
            edge.SetSelectedMaterial(edgeMaterial);
        }
    }

    public IEnumerable<PolyhedronEdge> GetEdges()
    {
        return edges;
    }

    #endregion
    public bool NearRotationAxis(Vector3 v, float error)
    {
        //return absPolyhedron.NearRotationAxis(v, error);
        return false;
    }

    #region --------------------- vertices ---------------------
    // TODO: Right now, they are not rotating

    public void AddVertices(Transform vertexPrefab)
    {
        GameObject vertices = new GameObject("Vertices");
        vertices.transform.parent = transform;
        vertices.transform.localPosition = Vector3.zero;
        foreach (var v in absPolyhedron.GetVertices())
        {
            Transform vertex = Instantiate<Transform>(vertexPrefab, vertices.transform);
            vertex.localPosition = v;
        }
    }

    #endregion
}
