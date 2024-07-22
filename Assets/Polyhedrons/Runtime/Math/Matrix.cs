using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix
{

    public static Matrix Identity(int d)
    {
        if (d < 1)
            d = 1;
        float[,] entries = new float[d, d];
        for (int i = 0; i < d; i++)
        {
            entries[i, i] = 1;
        }
        return new Matrix(d, d, entries);
    }

    public static Matrix Zero(int rows, int cols)
    {
        return new Matrix(rows, cols, new float[rows, cols]);
    }

    private float[,] entries;

    public Matrix(int rows, int cols, params float[] entries)
    {
        if (rows<=0 || cols <=0 || entries.Length != rows * cols)
        {
            Debug.Log("Bad parameters for the matrix");
            return;
        }

        this.entries = new float[rows, cols];
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                this.entries[i, j] = entries[index];
                index++;
            }
        }
    }

    public Matrix(int rows, int cols, float[,] entries)
    {
        if (rows <= 0 || cols <= 0 || entries.GetLength(0) != rows || entries.GetLength(1) != cols)
        {
            Debug.Log("Bad parameters for the matrix");
            return;
        }

        this.entries = new float[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                this.entries[i, j] = entries[i, j];
            }
        }
    }

    public int RowLength()
    {
        return entries.GetLength(0);
    }

    public int ColLength()
    {
        return entries.GetLength(1);
    }

    public float At(int row, int col)
    {
        return entries[row, col];
    }

    public void Set(int row, int col, float value)
    {
        entries[row, col] = value;
    }

    public float this[int row, int col]
    {
        get { return At(row, col); }
        set { Set(row, col, value); }
    }

    public string DimensionStr()
    {
        return $"[{RowLength()},{ColLength()}]";
    }

    public static Matrix operator *(Matrix elem1, Matrix elem2)
    {
        if (elem1.ColLength() != elem2.RowLength())
        {
            Debug.LogError($"Cannot multiply the given matrices {elem1.DimensionStr()}x{elem2.DimensionStr()} .");
            return null;
        }

        int n = elem1.RowLength();
        int K = elem1.ColLength();
        int m = elem2.ColLength();
        float[,] entries = new float[n, m];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                for (int k = 0; k < K; k++)
                {
                    entries[i, j] += elem1.At(i, k) * elem2.At(k, j);
                }
            }
        }

        return new Matrix(n, m, entries);
    }

    delegate float Operation(float a, float b);

    private static Matrix PointwiseOperation(Matrix elem1, Matrix elem2, Operation op)
    {
        int n = elem1.RowLength();
        int m = elem1.ColLength();

        if ((elem2.RowLength() != n) || (elem2.ColLength() != m))
        {
            Debug.LogError($"Cannot add the given matrices {elem1.DimensionStr()}x{elem2.DimensionStr()} .");
            return null;
        }

        float[,] entries = new float[n, m];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                entries[i, j] += op(elem1.At(i, j), elem2.At(i, j));
            }
        }

        return new Matrix(n, m, entries);
    }

    public static Matrix operator +(Matrix elem1, Matrix elem2)
    {
        return PointwiseOperation(elem1, elem2, (a, b) => a + b);
    }

    public static Matrix operator -(Matrix elem1, Matrix elem2)
    {
        return PointwiseOperation(elem1, elem2, (a, b) => a - b);
    }

    public Matrix Transpose()
    {
        int n = RowLength();
        int m = ColLength();

        float[,] entriesTr = new float[m, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                entriesTr[j, i] += entries[i, j];
            }
        }

        return new Matrix(m, n, entriesTr);
    }

    public bool Equals(Matrix matrix, float error)
    {
        int n = RowLength();
        int m = ColLength();

        if ((matrix.RowLength() != n) || (matrix.ColLength() != m))
        {
            Debug.LogError($"Cannot add the given matrices {DimensionStr()}x{matrix.DimensionStr()} .");
            return false;
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (Math.Abs(entries[i, j] - matrix.At(i, j)) > error)
                    return false;
            }
        }

        return true;
    }

    public bool Equals(Matrix matrix)
    {
        return Equals(matrix, 0);
    }

    public static Matrix Rotation2(float angle)
    {
        return new Matrix(2, 2,
            Mathf.Cos(angle),  Mathf.Sin(angle),
            -Mathf.Sin(angle), Mathf.Cos(angle));
    }
    
    public static Matrix Rotation3(float x, float y, float z, float angle)
    {
        Matrix id = Matrix.Identity(3);
        Vector3 axis = new Vector3(x, y, z);
        axis.Normalize();

        Vector3 perp1 = new Vector3(1, 0, 0);
        if (x != 0)
        {
            perp1.Set(-y, x, 0);
            perp1.Normalize();
        }

        Vector3 perp2 = Vector3.Cross(axis, perp1);

        Matrix baseChange = new Matrix(3, 3,
            axis.x,  axis.y,  axis.z,
            perp1.x, perp1.y, perp1.z,
            perp2.x, perp2.y, perp2.z);

        Matrix rotation = new Matrix(3, 3,
            1,       0,                 0,
            0, Mathf.Cos(angle),  Mathf.Sin(angle),
            0, -Mathf.Sin(angle), Mathf.Cos(angle));


        return baseChange.Transpose() * rotation * baseChange;
    }
}
