using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="VisualPolyheronProperties", menuName ="Properties/VisualPolyhedron")]
public class VisualPolyhedronProperties : ScriptableObject
{
    [Header("Faces")]
    [SerializeField] public float faceRadius = 1;
    [SerializeField] public Material faceMaterial;
    [SerializeField] public Material rootMaterial;
    [Header("Edges")]
    [SerializeField] public float edgeRadius = 0.1f;
    [SerializeField] public bool showEdges = true;
    [SerializeField] public Material edgeMaterial;
    [SerializeField] public Material selectedEdgeMaterial;
    [Header("Numbers")]
    [SerializeField] public float numberRadius = 2;
    [SerializeField] public bool showNumbers = false;
    [SerializeField] public float numberLabelDistance = 0.003f;
}
