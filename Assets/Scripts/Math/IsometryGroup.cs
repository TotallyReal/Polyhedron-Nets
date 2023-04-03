using MathNet.Numerics.LinearAlgebra;
using Complex = System.Numerics.Complex;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using System.Reflection;

/// <summary>
/// Used to model a finite isometry group of R^3 .
/// </summary>
public class IsometryGroup : FiniteGroup<IsoElement>
{

    public static readonly float ERROR = 0.01f;

    protected override IsoElement CreateIdentity()
    {
        IsoElement id = new IsoElement(this, Matrix.Identity(3), "Id");
        return id;
    }


    public static IsometryGroup GenerateFullGroup(params Matrix[] generators)
    {
        IsometryGroup group = new IsometryGroup();
        //List<IsoElement> sphere = new List<IsoElement>();
        //sphere.Add(group.identity);

        List<IsoElement> isoGenerators = new List<IsoElement>();
        int index = 1;
        foreach (var matrix in generators)
        {
            isoGenerators.Add(group.AddElement(new IsoElement(group, matrix, $"g_{index}")));
            index++;
        }

        group.Subgroup(isoGenerators.ToArray());

        return group;
    }

    public override IsoElement TryGet(IsoElement elem)
    {
        foreach (var element in this)
        {
            if (element.matrix.Equals(elem.matrix, ERROR))
            {
                return element;
            }
        }

        return null;
    }

    protected override IsoElement SimpleMultiply(IsoElement elem1, IsoElement elem2)
    {
        return new IsoElement(this, elem1.matrix * elem2.matrix, $"{elem1.Name}*{elem2.Name}");
    }

    public IsoElement AddElement(Matrix matrix)
    {
        return AddElement(new IsoElement(this, matrix));
    }
}

public class IsoElement
{

    public Matrix matrix;
    private IsometryGroup group;
    public string Name { get; private set; }

    public IsoElement(IsometryGroup group, Matrix matrix, string name = null)
    {
        this.matrix = matrix;
        this.group = group;
        if (name == null)
            this.Name = $"Elem({this.group.Size()})";
        else
            this.Name = name;
    }

    public static IsoElement operator *(IsoElement elem1, IsoElement elem2)
    {
        if (elem1.group != elem2.group)
        {
            Debug.LogError($"Elements belong to different groups");
            return null;
        }

        return elem1.group.Multiply(elem1, elem2);
    }

    public static Vector3 operator *(IsoElement elem, Vector3 v)
    {
        Matrix m = elem.matrix;
        return new Vector3(
            m[0, 0] * v.x + m[0, 1] * v.y + m[0, 2] * v.z,
            m[1, 0] * v.x + m[1, 1] * v.y + m[1, 2] * v.z,
            m[2, 0] * v.x + m[2, 1] * v.y + m[2, 2] * v.z
            );
    }

    public override string ToString()
    {
        return Name;
    }
    /*
    private Vector3 findAxis(Matrix<double> matrix)
    {

        // Compute the SVD of the matrix
        MathNet.Numerics.LinearAlgebra.Factorization.Svd<double> svd = matrix.Svd();

        // Find the kernel of the matrix
        double smallestEigenvalue = svd.S.Last();
        Vector<double> eigenVector = svd.VT.Row(2);
        /*Vector<double> eigenVector1, ev1;
        Vector<double> eigenVector2, ev2;
        Vector<double> eigenVector3, ev3;
        if (smallestEigenvalue < IsometryGroup.ERROR)
        {
            eigenVector1 = svd.VT.Column(0);
            eigenVector2 = svd.VT.Column(1);
            eigenVector3 = svd.VT.Column(2);
            ev1 = svd.VT.Row(0);
            ev2 = svd.VT.Row(1);
            ev3 = svd.VT.Row(2);
        }*/
        //return Vector3.zero;
        // Print the kernel
        /*Console.WriteLine("The kernel of the matrix is:");
        Console.WriteLine(kernel.ToString());



        Vector3 v = MathTools.RandomVector3(0.5f, 1f, 0.5f, 1f, 0.5f, 1f);
        Vector3 u = this * v;
        if ((v - u).magnitude < IsometryGroup.ERROR)
        {
            // found an eigenvector for 1 (though quite unlikely...)
            return v.normalized;
        }
        if ((v + u).magnitude < IsometryGroup.ERROR)
        {
            // found an eigenvector for -1 (though quite unlikely...)
            // ignore this for a second...
        }

        Vector3 w = this * u;
        // since v!=u,-u, then [v,u] and [u,w]
        return w;*/
    //}

    private int FirstIndex<T>(IEnumerable<T> items, Func<T, bool> matchCondition)
    {
        int index = 0;
        foreach (var item in items)
        {
            if (matchCondition.Invoke(item))
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    private bool AlmostEqual(Complex z, Complex w, float error)
    {
        return (z - w).Magnitude < error;
    }

    private bool represented = false;
    private Vector3 axis;
    private bool flip;

    public bool GetPresentation(out Vector3 axis/*, out float angle*/, out bool flip)
    {
        if (represented)
        {
            axis = this.axis;
            flip = this.flip;
            return true;
        }
        Matrix<float> A = Matrix<float>.Build.DenseOfArray(new float[,]
        {
            { matrix[0,0], matrix[0,1], matrix[0,2] },
            { matrix[1,0], matrix[1,1], matrix[1,2] },
            { matrix[2,0], matrix[2,1], matrix[2,2] },
        });

        MathNet.Numerics.LinearAlgebra.Factorization.Evd<float> evd = A.Evd();
        Vector<Complex> eigenValues = evd.EigenValues;
        Matrix<float> eigenVectors = evd.EigenVectors;

        // we work under the assumption that the matrix is orthogonal, so that 
        // it must have an eigenvalue +1 (resp. -1) if it has determinant +1 (resp -1)
        // and its eigenvector in general are perpendicular.

        // start by moving the +-1 eigenvalue and eigenvector to be the first one.
        if (A.Determinant() > 0)
        {
            int index = FirstIndex(eigenValues, z => AlmostEqual(z, Complex.One, IsometryGroup.ERROR));

            Complex tempEValue = eigenValues[0];
            eigenValues[0] = Complex.One; // = eigenValues[index] ... almost ...
            eigenValues[index] = tempEValue;

            MathNet.Numerics.LinearAlgebra.Vector<float> tempEVector = eigenVectors.Column(0);
            eigenVectors.SetColumn(0, eigenVectors.Column(index));
            eigenVectors.SetColumn(index, tempEVector);

            flip = false;
            /*// is a rotation
            eigenValues.Where(z => { return (z - Complex.One).Magnitude < IsometryGroup.ERROR; });
            eigenValues.Find
            for (int i = 0; i < 3; i++)
            {
                if ((eigenValues[i] - Complex.One).Magnitude < IsometryGroup.ERROR)
                {
                    Complex tempEV = eigenValues[0];
                    eigenValues[0] = Complex.One;
                    eigenValues[i] = tempEV;
                }
            }*/
        } else
        {
            // is a reflection + rotation
            int index = FirstIndex(eigenValues, z => AlmostEqual(z, -Complex.One, IsometryGroup.ERROR));

            Complex tempEValue = eigenValues[0];
            eigenValues[0] = -Complex.One; // = eigenValues[index] ... almost ...
            eigenValues[index] = tempEValue;

            MathNet.Numerics.LinearAlgebra.Vector<float> tempEVector = eigenVectors.Column(0);
            eigenVectors.SetColumn(0, eigenVectors.Column(index));
            eigenVectors.SetColumn(index, tempEVector);

            flip = true;
        }
        axis = new Vector3(eigenVectors[0, 0], eigenVectors[1, 0], eigenVectors[2, 0]);
        /*Matrix<float> A = Matrix<float>.Build.DenseOfArray(new float[,]
        {
            { 0f,1f,0f },
            { -3f,2f,1f },
            { 0f,-3f,4f },
        });*/
        //float eigenvalue = 1f;
        //MathTools.TryApproximateEigenvalue(A, eigenvalue, 3, IsometryGroup.ERROR, out Vector<float> eigenvector);
        //findAxis(A);
        //Vector<double>[] vectors = A.Kernel();
        //Vector<double> b = Vector<double>.Build.Dense(new double[] { 8.0, -11.0, -3.0 });

        // Solve the system of equations Ax = b
        //Vector<double> x = A.Solve(b);

        /*flip = false;
        axis = Vector3.zero;
        angle = 0;*/
        this.axis = axis;
        this.flip = flip;
        this.represented = true;
        return true;
    }

}
