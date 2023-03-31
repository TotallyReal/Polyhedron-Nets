using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// Used to model a finite isometry group of R^3 .
/// </summary>
public class IsometryGroup : FiniteGroup<IsoElement>
{

    private static float ERROR = 0.01f;

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

}
