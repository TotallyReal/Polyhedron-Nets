using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Whenever the object is pressed, looks for the closest rotation axis, and rotate around it.
/// To use this component, the object must have a collider.
/// </summary>
public class ClickableWorldRotator : WorldRotator
{
    [Tooltip("Leave empty is the rotation is applied to this object")]
    [SerializeField] private Transform rotationControlObject;
    // TODO: I hate everything about this
    [SerializeField] private List<RotationOp> axes = new List<RotationOp>();
    [SerializeField] private bool debugLines;
    [Tooltip("Leave empty to use DefaultMouseTypeEvent.standard")]
    [SerializeField] private MouseTypeEvent mouseEvent;

    protected void Awake()
    {
        mouseEvent = (mouseEvent == null) ? DefaultMouseTypeEvent.standard : mouseEvent;
        if (rotationControlObject == null)
        {
            rotationControlObject = transform;
        }
    }


    public void SetRotationControlObject(Transform rotationControlObject)
    {
        if (rotationControlObject != null)
        {
            DeleteWaitinRotation();
            this.rotationControlObject = rotationControlObject;
        }
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
                Debug.DrawLine(transform.position, transform.TransformPoint(rotationOp.normalizedAxis * 10), color: Color.white);
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

    private void Rotate(object sender, (Transform obj, Vector3 position) args)
    {
        if (args.obj == rotationControlObject)
        {
            Vector3 dir = args.position - transform.position;

            RotateAround(ClosestRotation(dir));
        }
    }

    #endregion



}
