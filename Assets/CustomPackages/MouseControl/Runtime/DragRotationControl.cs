using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Use the drag input from a drag object to rotate this object (which may or may not contain the drag object itself).
 */
public class DragRotationControl : MonoBehaviour
{

    [SerializeField] private DragAndDrop controlDragObject;
    [SerializeField] private bool axisBound = false;
    [SerializeField] private Vector3 axis = Vector3.up;

    private void Awake()
    {
        axis.Normalize();
    }

    private void OnEnable()
    {
        controlDragObject.OnStartDragging += OnStartDragging;
        controlDragObject.OnDragging += OnDragging;
    }

    private void OnDisable()
    {
        controlDragObject.OnStartDragging -= OnStartDragging;
        controlDragObject.OnDragging -= OnDragging;
    }

    private Vector3 previous = Vector3.zero;


    private void OnStartDragging(object sender, Vector3 v)
    {
        previous = v - controlDragObject.transform.position;
        if (axisBound)
        {
            previous -= Vector3.Dot(previous, axis) * axis;
            previous.Normalize();
        }
    }

    private void OnDragging(object sender, Vector3 v)
    {
        v -= controlDragObject.transform.position;
        Vector3 rotationAxis;
        if (axisBound)
        {
            v -= Vector3.Dot(v, axis) * axis;
            v.Normalize();
            rotationAxis = axis;
        }
        else
        {
            rotationAxis = Vector3.Cross(previous, v);
            if (rotationAxis.sqrMagnitude < 0.000001)
            {
                previous = v;
                return;
            }
        }

        float angle = Vector3.SignedAngle(previous, v, rotationAxis);
        transform.Rotate(rotationAxis, angle, Space.World);
        previous = v;
    }
}
