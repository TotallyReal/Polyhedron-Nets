using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UIElements;
using System.Xml.Linq;

public class AbstractPolyhedron
{

    public static AbstractPolyhedron CreateCube(float radius)
    {
        AbstractPolyhedron polyhedron = new AbstractPolyhedron();

        polyhedron.AddVertex(new Vector3(+radius, +radius, +radius), "+++"); // 0
        polyhedron.AddVertex(new Vector3(+radius, -radius, +radius), "+-+"); // 1
        polyhedron.AddVertex(new Vector3(+radius, +radius, -radius), "++-"); // 2
        polyhedron.AddVertex(new Vector3(+radius, -radius, -radius), "+--"); // 3
        polyhedron.AddVertex(new Vector3(-radius, +radius, +radius), "-++"); // 4
        polyhedron.AddVertex(new Vector3(-radius, -radius, +radius), "--+"); // 5
        polyhedron.AddVertex(new Vector3(-radius, +radius, -radius), "-+-"); // 6
        polyhedron.AddVertex(new Vector3(-radius, -radius, -radius), "---"); // 7

        polyhedron.AddFace(new int[] { 1, 3, 7, 5 }, "0-0", Vector3.zero);
        polyhedron.AddFace(new int[] { 0, 1, 3, 2 }, "+00", Vector3.zero);
        polyhedron.AddFace(new int[] { 4, 5, 7, 6 }, "-00", Vector3.zero);
        polyhedron.AddFace(new int[] { 0, 2, 6, 4 }, "0+0", Vector3.zero);
        polyhedron.AddFace(new int[] { 0, 1, 5, 4 }, "00+", Vector3.zero);
        polyhedron.AddFace(new int[] { 2, 3, 7, 6 }, "00-", Vector3.zero);

        return polyhedron;
    }

    public static AbstractPolyhedron CreateDiamond(float radius)
    {
        AbstractPolyhedron polyhedron = new AbstractPolyhedron();

        polyhedron.AddVertex(new Vector3(0, 0, +radius), "00+"); // 0
        polyhedron.AddVertex(new Vector3(0, 0, -radius), "00-"); // 1
        polyhedron.AddVertex(new Vector3(0, +radius, 0), "0+0"); // 2
        polyhedron.AddVertex(new Vector3(0, -radius, 0), "0-0"); // 3
        polyhedron.AddVertex(new Vector3(+radius, 0, 0), "+00"); // 4
        polyhedron.AddVertex(new Vector3(-radius, 0, 0), "-00"); // 5

        polyhedron.AddFace(new int[] { 0, 2, 4 }, "+++", Vector3.zero);
        polyhedron.AddFace(new int[] { 0, 2, 5 }, "++-", Vector3.zero);
        polyhedron.AddFace(new int[] { 0, 3, 4 }, "+-+", Vector3.zero);
        polyhedron.AddFace(new int[] { 0, 3, 5 }, "+--", Vector3.zero);
        polyhedron.AddFace(new int[] { 1, 2, 4 }, "-++", Vector3.zero);
        polyhedron.AddFace(new int[] { 1, 2, 5 }, "-+-", Vector3.zero);
        polyhedron.AddFace(new int[] { 1, 3, 4 }, "--+", Vector3.zero);
        polyhedron.AddFace(new int[] { 1, 3, 5 }, "---", Vector3.zero);

        return polyhedron;
    }

    struct AbstractVertex
    {
        public Vector3 position;
        public string name;
    }

    struct AbstractEdge
    {
        public int vertex1;
        public int vertex2;
        public AbstractFace face1;
        public AbstractFace face2;
    }

    public struct AbstractFace 
    {
        public List<int> vertices;
        public string name;

        public void Flip()
        {
            vertices.Reverse();
        }

        public int VertexCount()
        {
            return vertices.Count;
        }

        public IEnumerable<int> VertexIndices()
        {
            return vertices;
        }
        public IEnumerable<(int, int)> EdgesIndices()
        {
            int[] vertexIndices = vertices.ToArray();
            for (int i = 0; i < vertexIndices.Length - 1; i++)
            {
                yield return (vertexIndices[i], vertexIndices[i + 1]);
            }
            yield return (vertexIndices[vertexIndices.Length - 1], vertexIndices[0]);
        }
    }

    private List<AbstractVertex> vertices;

    private List<AbstractFace> faces;

    private Dictionary<(int, int), AbstractEdge> edges;

    private bool TryGetEdge(int v1, int v2, out AbstractEdge abstractEdge)
    {
        if (edges.TryGetValue((v1, v2), out abstractEdge) || edges.TryGetValue((v2, v1), out abstractEdge))
        {
            return true;
        }
        return false;
    }

    public AbstractPolyhedron()
    {
        vertices = new List<AbstractVertex>();
        faces = new List<AbstractFace>();
        edges = new Dictionary<(int, int), AbstractEdge>();
    }

    public int AddVertex(Vector3 position, string name)
    {
        int index = vertices.Count;
        name ??= "" + index;
        vertices.Add(new AbstractVertex { position = position, name = name });
        return index;
    }

    public int AddVertex(Vector3 position)
    {
        return AddVertex(position, null);
    }

    private Vector3 GetVertex(AbstractFace face, int index)
    {
        return vertices[face.vertices[index]].position;
    }

    // only works for convex faces
    private Vector3 Normal(AbstractFace face)
    {
        Vector3 v = GetVertex(face, 1) - GetVertex(face, 0);
        Vector3 u = GetVertex(face, 2) - GetVertex(face, 0);
        return Vector3.Cross(v, u).normalized;
    }

    private AbstractFace AddReturnFace(IEnumerable<int> vertices, string name)
    {
        name ??= "" + faces.Count;
        List<int> verticesCopy = (from v in vertices select v).ToList();
        AbstractFace abstractFace = new AbstractFace { vertices = verticesCopy, name = name };
        faces.Add(abstractFace);

        int[] vertexIndices = abstractFace.vertices.ToArray();
        //Debug.Log($"Added face for vertices {vertexIndices[0]}, {vertexIndices[1]}, {vertexIndices[2]}, {vertexIndices[3]}, {vertexIndices[4]}");
        for (int i = 0; i < vertexIndices.Length; i++)
        {
            int j = (i == 0) ? vertexIndices.Length - 1 : i - 1;
            if (TryGetEdge(vertexIndices[j], vertexIndices[i],out AbstractEdge abstractEdge))
            {
                abstractEdge.face2 = abstractFace;
                //Debug.Log($"a - - - Edge ({vertexIndices[j]},{vertexIndices[i]}) - face 2");
            } else
            {
                //Debug.Log($"b - - - Edge ({vertexIndices[j]},{vertexIndices[i]}) - face 1");
                abstractEdge = new AbstractEdge { 
                    vertex1 = vertexIndices[j], vertex2 = vertexIndices[i], face1 = abstractFace 
                };
                edges.Add((vertexIndices[j], vertexIndices[i]), abstractEdge);
            }
        }

        return abstractFace;
    }

    protected void MoveLastFaceToFirst()
    {
        AbstractFace abstractFace = faces[faces.Count-1];
        faces.RemoveAt(faces.Count - 1);
        faces.Insert(0, abstractFace);
    }

    /// <summary>
    /// Assuming that the vertices used in this face are already defined, use them to define
    /// the normal to this face, and if necessary flip the face so the given interior point
    /// will be in the negative side of the normal.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="name"></param>
    /// <param name="interiorPoint"></param>
    public void AddFace(IEnumerable<int> vertices, string name, Vector3 interiorPoint)
    {
        AbstractFace face = AddReturnFace(vertices, name);
        Vector3 normal = Normal(face);
        Vector3 dirToFace = GetVertex(face, 0) - interiorPoint;
        if (Vector3.Dot(normal, dirToFace) < 0)
        {
            face.Flip();
        }
    }

    public void AddFace(IEnumerable<int> vertices, string name)
    {
        AddReturnFace(vertices, name);
    }

    public void AddFace(IEnumerable<int> vertices)
    {
        AddFace(vertices, null);
    }

    public List<int[]> GetFaces()
    {
        return (from face in faces
                select face.vertices.ToArray()).ToList();
    }

    public IEnumerable<AbstractFace> GetAbstractFaces()
    {
        return faces;
    }

    internal Vector3[] GetVertices()
    {
        return (from v in vertices
                select v.position).ToArray<Vector3>();
    }

}
