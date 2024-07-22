using MathNet.Numerics.LinearAlgebra;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static public class MathTools {

    /// <summary>
    /// Rotate the given object (in transform), so that the vector "fromVector" would be rotated to be "toVector".
    /// The inWorldSpace indicates if these vectors are given in world space coordinates, or local.
    /// </summary>
    public static void RotateToMatch(Transform transform, Vector3 fromVector, Vector3 toVector, bool inWorldSpace = true)
    {
        Vector3 rotationAxis = Vector3.Cross(toVector, fromVector);
        //rotationAxis.Normalize();
        float angle = Vector3.SignedAngle(fromVector, toVector, rotationAxis);
        if (inWorldSpace)
            rotationAxis = transform.InverseTransformDirection(rotationAxis);
        transform.Rotate(rotationAxis, angle);
    }

    public static Vector3 RandomVector3(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        return new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ)
            );
    }

    public static bool TryApproximateEigenvalue(
        Matrix<float> matrix, float eigenvalue, int dim, float error, out Vector<float> eigenvector) 
    {
        matrix -= eigenvalue * Matrix<float>.Build.SparseIdentity(dim, dim);
        MathNet.Numerics.LinearAlgebra.Factorization.Svd<float> svd = matrix.Svd();
        double smallestEigenvalue = svd.S.Last();
        if (smallestEigenvalue < error)
        {
            eigenvector = svd.VT.Row(dim - 1);
            return true;
        }
        eigenvector = null;
        return false;
    }

    public static int[] RandomPermutation(int n)
    {
        int[] numbers = new int[n];
        for (int i = 0; i < n; i++)
        {
            numbers[i] = i;
        }

        return numbers.OrderBy(x => Random.Range(0f,1f)).ToArray();
    }

    public static IEnumerable<T> RandomList<T>(List<T> elements)
    {
        int[] numbers = RandomPermutation(elements.Count);
        foreach (int i in numbers)
        {
            yield return elements[i];
        }
    }

}
