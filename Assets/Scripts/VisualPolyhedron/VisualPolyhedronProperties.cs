using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="VisualPolyheronProperties", menuName ="Properties/VisualPolyhedron")]
public class VisualPolyhedronProperties : ScriptableObject
{
    [Header("Faces")]
    [SerializeField] private FaceMesh facePrefab;
    [SerializeField] private float faceRadius = 5;
    [SerializeField] private Material faceMaterial;
    [SerializeField] private Material rootMaterial;
    [Header("Edges")]
    [SerializeField] private PolyhedronEdge axisPrefab;
    [SerializeField] private float edgeRadius = 0.5f;
    [SerializeField] private bool showEdges = true;
    [SerializeField] private Material edgeMaterial;
    [Header("Numbers")]
    [SerializeField] private NumberedCanvas numberedCanvasPrefab;
    [SerializeField] private float numberRadius = 2;
    [SerializeField] private bool showNumbers = false;
    [SerializeField] private float numberLabelDistance = 0.003f;
}
