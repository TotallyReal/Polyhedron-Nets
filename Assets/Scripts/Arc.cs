using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Arc : MonoBehaviour
{

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Mesh mesh;
    [Range(0, 20)]
    [SerializeField] private float outerRadius = 5f;
    [Range(1, 45)]
    [SerializeField] private int outerCircleResolution = 20;
    [Range(0, 20)]
    [SerializeField] private float innerRadius = 1f;
    [Range(1, 25)]
    [SerializeField] private int innerCircleResolution = 5;
    [Range(0, 2 * Mathf.PI)]
    [SerializeField] private float angle = Mathf.PI / 3f;

    private void Awake()
    {
        CreateMesh();
    }

    public void SetParameters(float angle, float outerRadius, float innerRadius)
    {
        this.outerRadius = (outerRadius > 0) ? outerRadius : 0;
        this.innerRadius = (innerRadius > 0) ? innerRadius : 0;
        this.angle = angle % (2 * Mathf.PI);

        CreateMesh();
    }

    public void SetAngle(float angle)
    {
        this.angle = angle % (2 * Mathf.PI);

        CreateMesh();
    }


    private int VertexIndex(int outerRadiusIndex, int innerRadiusIndex)
    {
        return outerRadiusIndex * innerCircleResolution + innerRadiusIndex;
    }


    public void CreateMesh()
    {
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            mesh = new Mesh();
            meshFilter.mesh = mesh;

            meshCollider = GetComponent<MeshCollider>();
        }

        Vector3[] vertices = new Vector3[innerCircleResolution * outerCircleResolution];

        for (int i = 0; i < outerCircleResolution; i++)
        {
            float bigAngle = (angle * i) / (outerCircleResolution-1);
            Vector3 radial = new Vector3(Mathf.Cos(bigAngle), 0, Mathf.Sin(bigAngle));
            for (int j = 0; j < innerCircleResolution; j++)
            {
                float smallAngle = (2 * Mathf.PI * j) / innerCircleResolution;
                vertices[VertexIndex(i, j)] = outerRadius * radial + innerRadius * (Mathf.Cos(smallAngle) * radial + Mathf.Sin(smallAngle) * Vector3.up);
            }
        }

        // For each point we have a different sqaure, so there are #points * 2 triangles.
        // In addition, there are the two caps which are both faces with innerCircleResolution vertices,
        // so they require innerCircleResolution-2 triangles
        int[] triangleIndices = new int[
            innerCircleResolution * outerCircleResolution * 2 * 3 +
            (innerCircleResolution - 2) * 2 * 3];

        // torus triangles
        int triIndex = 0;
        for (int i = 1; i < outerCircleResolution; i++)
        {
            int iMinus = (i == 0) ? outerCircleResolution-1 : i - 1;
            for (int j = 0; j < innerCircleResolution; j++)
            {
                int jMinus = (j == 0) ? innerCircleResolution - 1 : j - 1;

                triangleIndices[triIndex + 0] = VertexIndex(i, j);
                triangleIndices[triIndex + 1] = VertexIndex(iMinus, jMinus);
                triangleIndices[triIndex + 2] = VertexIndex(iMinus, j);

                triangleIndices[triIndex + 3] = VertexIndex(i, j);
                triangleIndices[triIndex + 4] = VertexIndex(i, jMinus);
                triangleIndices[triIndex + 5] = VertexIndex(iMinus, jMinus);

                triIndex += 6;
            }
        }

        // caps
        for (int j = 0; j < innerCircleResolution - 2; j++)
        {
            triangleIndices[triIndex + 0] = VertexIndex(0, 0);
            triangleIndices[triIndex + 1] = VertexIndex(0, j + 2);
            triangleIndices[triIndex + 2] = VertexIndex(0, j + 1);

            triangleIndices[triIndex + 3] = VertexIndex(outerCircleResolution - 1, 0);
            triangleIndices[triIndex + 4] = VertexIndex(outerCircleResolution - 1, j + 1);
            triangleIndices[triIndex + 5] = VertexIndex(outerCircleResolution - 1, j + 2);

            triIndex += 6;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshCollider.sharedMesh = mesh;
    }
}
