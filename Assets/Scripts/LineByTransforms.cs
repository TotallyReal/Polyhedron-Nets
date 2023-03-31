using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class LineByTransforms : MonoBehaviour
{

    private LineRenderer lineRenderer;
    [SerializeField] private List<Transform> points;
    [SerializeField] private bool isClosedPolygon = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void UpdatePolygon()
    {
        foreach (var pt in points)
        {
            pt.hasChanged = false;
        }
        lineRenderer.positionCount = points.Count + (isClosedPolygon ? 1 : 0);
        lineRenderer.SetPositions((from pt in points select pt.position).ToArray());
    }
}
