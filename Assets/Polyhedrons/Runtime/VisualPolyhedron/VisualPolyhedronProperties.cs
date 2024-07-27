using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="VisualPolyheronProperties", menuName ="Properties/VisualPolyhedron")]
public class VisualPolyhedronProperties : ScriptableObject
{
    [Tooltip("The radius on which the vertices lie.")]
    [SerializeField] public float radius;
    [Header("Faces")]
    //[SerializeField] public float faceRadius = 1;
    [SerializeField] public Material faceMaterial;
    [SerializeField] public Material rootMaterial;
    [Header("Edges")]
    [Tooltip("The ratio is between the edge radius and the full polyhedron radius")]
    [SerializeField] public float edgeRadiusRatio = 0.1f;
    [SerializeField] public bool showEdges = true;
    [SerializeField] public Material edgeMaterial;
    [SerializeField] public Material selectedEdgeMaterial;
    [Header("Numbers")]
    [Tooltip("The ratio is between the number label radius and the full polyhedron radius")]
    [SerializeField] public float numberRadiusRatio = 2;
    [SerializeField] public bool showNumbers = false;
    [SerializeField] public float numberLabelDistance = 0.003f;
}
