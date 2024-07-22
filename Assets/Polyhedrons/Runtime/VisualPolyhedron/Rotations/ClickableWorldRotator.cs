using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;


/// <summary>
/// Whenever the rotation object is pressed, looks for the closest rotation axis, and rotate around it.
/// </summary>
public class ClickableWorldRotator : WorldRotator
{
    // TODO: I hate everything about this
    [SerializeField] private List<RotationOp> axes = new List<RotationOp>();
    private RaycastSelector.MouseEvent mouseEvent;

    // TODO: move somewhere else
    public static List<RotationOp> GetRotationsFromPolyhedron(AbstractGroupPolyhedron groupPoly)
    {
        List<RotationOp> axes = new List<RotationOp>();

        float verAngle = 360 / groupPoly.vertexDegree;
        foreach (var vertex in groupPoly.GetVertices())
        {
            axes.Add(new RotationOp(vertex, verAngle));
        }

        foreach (var face in groupPoly.GetFaces())
        {
            Vector3 center = Vector3.zero;
            // TODO: If vertex count is always the same, consider fixing this.
            int verCount = 0;
            foreach (Vector3 v in groupPoly.GetVerticesAt(face))
            {
                verCount += 1;
                center += v;
            }
            center /= 4;
            axes.Add(new RotationOp(center, 360 / verCount));
        }

        return axes;
    }

    void Awake()
    {
        mouseEvent = RaycastSelector.playerMouseEvent;
        SetRotationOp(GetRotationsFromPolyhedron(AbstractGroupPolyhedron.Cube(1)));
    }


    public void SetRotationOp(List<RotationOp> rotations)
    {
        if (rotations != null)
        {
            axes = rotations;
        }

    }


    #region -------------------- Action Event Rotation --------------------

    private void OnEnable()
    {
        mouseEvent.OnObjectSelectPlus += Rotate;
    }

    private void OnDisable()
    {
        mouseEvent.OnObjectSelectPlus -= Rotate;
    }

    private RotationOp ClosestRotation(Vector3 dir) {
        dir.Normalize();

        float dist = 1000; // > 2
        RotationOp closestRotOp = new RotationOp(Vector3.up, 0);
        foreach (RotationOp currentRotOp in axes)
        {
            float d = Vector3.Distance(dir, currentRotOp.normalizedAxis);
            if (d < dist)
            {
                closestRotOp = currentRotOp;
                dist = d;
            }
        }
        return closestRotOp;
    }

    private void Rotate(object sender, (Transform, RaycastHit) e)
    {
        if (e.Item1 == GetRotationObject())
        {
            Vector3 dir = e.Item2.point - transform.position;
            RotateAround(ClosestRotation(dir));
        }
    }

    #endregion



}
