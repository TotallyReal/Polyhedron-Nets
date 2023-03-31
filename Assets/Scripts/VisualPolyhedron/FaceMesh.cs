using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMesh : Face
{
    private float tileScale = 5f;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Mesh mesh;
    public Vector3 LocalNormal { get; private set; }
    public Vector3 Center { get; private set; }

    public void SetMaterial(Material material)
    {
        GetComponent<Renderer>().material = material;
    }

    private void UpdateCenter(Vector3[] vertices)
    {
        Vector3 center = Vector3.zero;
        foreach (var v in vertices)
        {
            center += v;
        }
        Center = center / vertices.Length;
    }

    public void CreateMesh(params Vector3[] vertices)
    {
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            //meshFilter = gameObject.AddComponent<MeshFilter>();
            mesh = new Mesh();
            meshFilter.mesh = mesh;

            meshCollider = GetComponent<MeshCollider>();
            //gameObject.GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial; // new Material(Shader.Find("Standard"));
        }

        UpdateCenter(vertices);

        // Assume that the next two vectors are not colinear.
        Vector3 v = (vertices[1] - vertices[0]).normalized;
        Vector3 u = (vertices[2] - vertices[0]).normalized;
        LocalNormal = Vector3.Cross(v, u).normalized;
        u = Vector3.Cross(LocalNormal, v).normalized;
        // at this point {u,v} is an orthonormal basis for the plane parallel to the face.

        int n = vertices.Length;
        Vector3[] doubleVertices = new Vector3[2 * n];
        Array.Copy(vertices, 0, doubleVertices, 0, n);
        Array.Copy(vertices, 0, doubleVertices, n, n);

        Vector2[] uvs = new Vector2[doubleVertices.Length];
        for (int i = 0; i < doubleVertices.Length; i++)
        {
            Vector3 p = doubleVertices[i] - vertices[0];
            uvs[i] = new Vector2(Vector3.Dot(p, v)/ tileScale, Vector3.Dot(p, u)/tileScale);
        }

        int[] triangles = new int[6 * (vertices.Length-2)];
        int triIndex = 0;
        for (int i = 2; i < n; i++)
        {
            triangles[triIndex + 0] = 0;
            triangles[triIndex + 1] = i - 1;
            triangles[triIndex + 2] = i;

            // reverse triangle
            triangles[triIndex + 3] = n + 0;
            triangles[triIndex + 4] = n + i;
            triangles[triIndex + 5] = n + i - 1;
            triIndex += 6;
        }

        mesh.Clear();
        mesh.vertices = doubleVertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshCollider.sharedMesh = mesh;
    }


    /// <summary>
    /// Returns the normal in the world space
    /// </summary>
    /// <returns>Normal in world space</returns>
    public override Vector3 Normal()
    {
        return transform.TransformDirection(LocalNormal);
    }
}
