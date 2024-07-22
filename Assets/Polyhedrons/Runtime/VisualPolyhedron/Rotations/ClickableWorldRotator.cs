using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static WorldRotator;


/// <summary>
/// Whenever the object is pressed, looks for the closest rotation axis, and rotate around it.
/// To use this component, the object must have a collider.
/// </summary>
public class ClickableWorldRotator : WorldRotator
{
    // TODO: I hate everything about this
    [SerializeField] private List<RotationOp> axes = new List<RotationOp>();
    [SerializeField] private bool debugLines;
    private RaycastSelector.MouseEvent mouseEvent;

    protected override void Awake()
    {
        base.Awake();
        mouseEvent = RaycastSelector.playerMouseEvent;
    }


    public void SetRotationOp(List<RotationOp> rotations)
    {
        if (rotations != null)
        {
            axes = rotations;
        }

    }

    private void Update()
    {
        if (debugLines)
        {
            foreach (RotationOp rotationOp in axes)
            {
                Debug.DrawLine(new Vector3(0, 0, 0), rotationOp.normalizedAxis * 10, color: Color.white);
            }
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
