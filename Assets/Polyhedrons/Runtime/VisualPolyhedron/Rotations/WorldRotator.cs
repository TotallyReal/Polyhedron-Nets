using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;


/// <summary>
/// Whenever the rotation object is pressed, look for the closest rotation axis, and rotate around it.
/// </summary>
public class WorldRotator : MonoBehaviour
{

    public class RotationOp
    {
        public RotationOp(Vector3 axis, float angle)
        {
            normalizedAxis = axis.normalized;
            this.angle = angle;
        }

        // TODO: I hate serializing objects in unity... This is a nightmare
        [SerializeField] public Vector3 normalizedAxis;
        [SerializeField] public float angle;

        public override string ToString() => $"{normalizedAxis}[{angle}]";
    }


    [SerializeField] private Transform rotationObject;

    [SerializeField] private float rotationTimeSec = 1f;

    private RotationOp waitingRotation = null;
    private bool isRotating;

    public Transform GetRotationObject()
    {
        return rotationObject;
    }

    private void FinishedRotating()
    {
        isRotating = false;
        if (waitingRotation != null)
        {
            RotationOp nextRotation = waitingRotation;
            waitingRotation = null;
            RotateAround(nextRotation);
        }
    }

    public void RotateAround(RotationOp rotation)
    {
        if (waitingRotation!=null)
        {
            return;
        }
        if (isRotating) // TODO: Consider saving the action, only if near the end of the previous action.
        {
            waitingRotation = rotation;
            return;
        }
        isRotating = true;

        Quaternion targetRotation = Quaternion.AngleAxis(rotation.angle, rotation.normalizedAxis) * transform.rotation;

        transform.DORotateQuaternion(targetRotation, rotationTimeSec)
            .SetRelative(false)
            .OnComplete(FinishedRotating);        
    }

}
