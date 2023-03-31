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

    private void Awake()
    {
        edges = new List<Axis>();
        faces = new List<FaceMesh>();
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

    public void AddEdge(Axis axis) {
        edges.Add(axis);
    }

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

    public void SetEdgeMaterial(Material edgeMaterial)
    {
        this.edgeMaterial = edgeMaterial;
        foreach (var edge in edges)
        {
            edge.SetMaterial(edgeMaterial);
        }
    }
}
