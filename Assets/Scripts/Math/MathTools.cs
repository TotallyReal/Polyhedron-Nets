using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

}
