using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubicPolyhedron : AbstractPolyhedron
{
       

    public CubicPolyhedron(Vector3Int[] cubePositions, Vector3Int rootSide1, Vector3Int rootSide2, float radius)
    {
        if (rootSide1.x > rootSide2.x || rootSide1.y > rootSide2.y || rootSide1.z > rootSide2.z)
        {
            Vector3Int tempVector = rootSide1;
            rootSide1 = rootSide2;
            rootSide2 = tempVector;
        }

        // In the computation below, we think of a cube in position (i,j,k) in the input, as
        // a physical cube centered around (2i,2j,2k) with radius 1, so that its indices are
        // in (2i+-1, 2j+-1, 2k+-1) are in odd positions.
        int M = 0;
        HashSet<Vector3Int> cubePositionsHash = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, int> vertexToIndex = new Dictionary<Vector3Int, int>();
        HashSet<Vector3Int> vertices = new HashSet<Vector3Int>();
        foreach (Vector3Int position in cubePositions)
        {
            M = Mathf.Max(M, Mathf.Abs(position.x));
            M = Mathf.Max(M, Mathf.Abs(position.y));
            M = Mathf.Max(M, Mathf.Abs(position.z));
            cubePositionsHash.Add(position);
        }
        M += 1;

        // If necessary, adds the vertex to vertexToIndex, and returns its index
        int TryAddVertex(Vector3Int position)
        {
            if (!vertexToIndex.TryGetValue(position, out int index))
            {
                index = AddVertex(new Vector3(position.x, position.y, position.z) * radius);
                vertexToIndex.Add(position, index);
                //Debug.Log($"Adding vertex at {position} with index {index}");
            }
            return index;
        }

        void TryAddFace(Vector3Int position, Vector3Int dir, Vector3Int perp1, Vector3Int perp2)
        {
            bool current = cubePositionsHash.Contains(position);
            bool next = cubePositionsHash.Contains(position + dir);
            if (current ^ next)
            {
                Vector3Int cubeCenter = current ? position : position + dir;
                //Debug.Log($"Adding face {position} + {dir}");
                // Need to add a face. Check first if we already have the required vertices,
                // and if not, add them.
                Vector3Int center = 2 * position + dir;
                AddFace(new int[] {
                            TryAddVertex(center + perp1 + perp2),
                            TryAddVertex(center - perp1 + perp2),
                            TryAddVertex(center - perp1 - perp2),
                            TryAddVertex(center + perp1 - perp2)
                        }, $"({position}->{position+dir})", (Vector3)cubeCenter * 2 * radius);

                if (position == rootSide1 && position+dir == rootSide2)
                {
                    MoveLastFaceToFirst();
                }
            }
        }

        for (int i = -M; i < M; i++)
        {
            for (int j = -M; j <M; j++)
            {
                for (int k = -M; k < M; k++)
                {
                    Vector3Int position = new Vector3Int(i, j, k);
                    TryAddFace(position, Vector3Int.right, Vector3Int.up, Vector3Int.forward);                    
                    TryAddFace(position, Vector3Int.up, Vector3Int.forward, Vector3Int.right);
                    TryAddFace(position, Vector3Int.forward, Vector3Int.right, Vector3Int.up);
                    /*bool current = cubePositionsHash.Contains(new Vector3Int(i, j, k));
                    bool next = cubePositionsHash.Contains(new Vector3Int(i+1, j, k));
                    if (current ^ next)
                    {
                        // Need to add a face. Check first if we already have the required vertices,
                        // and if not, add them.
                        AddFace(new int[] {
                            TryAddVertex(new Vector3Int(2 * i + 1, 2 * j + 1, 2 * k + 1)),
                            TryAddVertex(new Vector3Int(2 * i + 1, 2 * j - 1, 2 * k + 1)),
                            TryAddVertex(new Vector3Int(2 * i + 1, 2 * j + 1, 2 * k - 1)),
                            TryAddVertex(new Vector3Int(2 * i + 1, 2 * j - 1, 2 * k - 1))
                        }, $"({i} ~ {i+1},{j},{k})");

                    }*/
                }
            }
        }
    }

}
