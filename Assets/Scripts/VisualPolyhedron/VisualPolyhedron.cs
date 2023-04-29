using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPolyhedron : MonoBehaviour
{

    [SerializeField] private Material faceMaterial;
    [SerializeField] private Material edgeMaterial;

    private List<Axis> edges;
    private List<FaceMesh> faces;
    public FaceMesh RootFace { get; set; }
    private FaceGraph faceGraph;

    private void Awake()
    {
        edges = new List<Axis>();
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

    public FaceGraph GetFaceGraph()
    {
        return faceGraph;
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
    public void AddEdge(Axis axis)
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

    public IEnumerable<Axis> GetEdges()
    {
        return edges;
    }

    #endregion
    public bool NearRotationAxis(Vector3 v, float error)
    {
        //return absPolyhedron.NearRotationAxis(v, error);
        return false;
    }
}
