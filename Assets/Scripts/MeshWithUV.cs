using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class MeshWithUV : MonoBehaviour
{

    public event EventHandler OnMeshCreated;

    [SerializeField] private List<Transform> vertices;
    [SerializeField] private List<Transform> uvs;
    [SerializeField] private LineByTransforms lineByTransforms;
    [SerializeField] private bool doubleSided = false;

    [SerializeField] private Material material;

    private MeshFilter meshFilter;
    private Mesh mesh;

    /*private void OnValidate()
    {
        CreateMesh(vertex1.position, vertex2.position, vertex3.position);
    }*/

    private void TrueUpdate()
    {
        foreach (var v in vertices)
        {
            v.hasChanged = false;
        }
        foreach (var v in uvs)
        {
            v.hasChanged = false;
        }
        
        CreateMesh();
    }

    private void Update()
    {
        if (vertices.Count != uvs.Count)
            return;
        foreach (var v in vertices)
        {
            if (v.hasChanged)
                TrueUpdate();
        }
        foreach (var v in uvs)
        {
            if (v.hasChanged)
                TrueUpdate();
        }
    }

    public void SetMaterial(Material material)
    {
        GetComponent<Renderer>().material = material;
        this.material = material;
    }

    private void OnValidate()
    {
        SetMaterial(material);
    }


    public void CreateMesh()
    {

        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            //meshFilter = gameObject.AddComponent<MeshFilter>();
            mesh = new Mesh();
            meshFilter.mesh = mesh;

            SetMaterial(material);
            //gameObject.GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial; // new Material(Shader.Find("Standard"));
        }

        mesh.Clear();

        Vector3[] verticesArray = (from v in vertices select v.position).ToArray();
        Vector3[] uvsArray = (from v in uvs select v.position).ToArray();

        int n = verticesArray.Length;
        if (doubleSided)
        {
            Vector3[] doubleVertices = new Vector3[2 * n];
            Array.Copy(verticesArray, 0, doubleVertices, 0, n);
            Array.Copy(verticesArray, 0, doubleVertices, n, n);
            mesh.vertices = doubleVertices;

            Vector3[] doubleUVs = new Vector3[2 * n];
            Array.Copy(uvsArray, 0, doubleUVs, 0, n);
            Array.Copy(uvsArray, 0, doubleUVs, n, n);
            mesh.uv = (from uv in doubleUVs select ToVector2(uv)).ToArray();
        } else
        {
            mesh.vertices = verticesArray;
            mesh.uv = (from uv in uvsArray select ToVector2(uv)).ToArray();
        }


        int[] triangles = new int[(doubleSided?2:1) * 3 * (n - 2)];
        int triIndex = 0;
        for (int i = 2; i < n; i++)
        {
            triangles[triIndex + 0] = 0;
            triangles[triIndex + 1] = i - 1;
            triangles[triIndex + 2] = i;
            triIndex += 3;

            if (doubleSided)
            {
                // reverse triangle
                triangles[triIndex + 3] = n + 0;
                triangles[triIndex + 4] = n + i;
                triangles[triIndex + 5] = n + i - 1;
                triIndex += 3;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        OnMeshCreated?.Invoke(this, EventArgs.Empty);
        if (lineByTransforms != null)
            lineByTransforms.UpdatePolygon();
    }

    Vector2 ToVector2(Vector3 v)
    {
        return v;
    }

}
