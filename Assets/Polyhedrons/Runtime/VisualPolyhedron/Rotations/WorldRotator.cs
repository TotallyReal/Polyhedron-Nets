using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;


/// <summary>
/// Applies rotation animation to the game object.
/// Can keep one rotation operation in waiting for the current rotation to finish (if exists).
/// </summary>
public class WorldRotator : MonoBehaviour
{
    [System.Serializable]
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

    [Tooltip("Leave empty is the rotation is applied to this object")]
    [SerializeField] private Transform rotationObject;
    [SerializeField] private float rotationTimeSec = 1f;

    private RotationOp waitingRotation = null;
    private bool isRotating;

    virtual protected void Awake()
    {
        if (rotationObject == null)
        {
            rotationObject = transform;
        }
    }

    public Transform GetRotationObject()
    {
        return rotationObject;
    }

    public void SetRotationObject(Transform rotationObject)
    {
        if (rotationObject != null)
        {
            this.rotationObject = rotationObject;
            waitingRotation = null;
            isRotating = false;
        }
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
