using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAxis : MonoBehaviour
{
    private Vector3 axisDirection = Vector3.up;
    private Vector3 axisPoint = Vector3.zero;
    private Vector3 axisDirectionLocal = Vector3.up;
    private Vector3 axisPointLocal = Vector3.zero;

    private void Awake()
    {
        InitRotationProcess();
    }

    private void Update()
    {
        RotationProcessUpdate();
    }

    #region ------------ Rotation process animation ------------

    private class RotationProcess
    {
        public Transform transform;
        public float angleLeft; // should be positive
        public int direction;
        public float anglePerSecond;
    }

    private List<RotationProcess> processes;

    private void InitRotationProcess()
    {
        processes = new List<RotationProcess>();
    }

    public void AddRotationProcess(Transform objTransform, float angle, float anglePerSecond)
    {
        processes.Add(new RotationProcess()
        {
            transform = objTransform,
            angleLeft = Mathf.Abs(angle),
            direction = (angle > 0) ? 1 : -1,
            anglePerSecond = anglePerSecond
        });
    }

    private void RotationProcessUpdate()
    {
        if (processes.Count == 0)
            return;
        UpdateGlobalParameters();
        foreach (var process in processes)
        {
            float dAngle = Mathf.Min(process.angleLeft, process.anglePerSecond * Time.deltaTime);
            process.angleLeft -= process.anglePerSecond * Time.deltaTime;
            RotateAround(process.transform, process.direction * dAngle);
        }
        processes.RemoveAll(process => (process.angleLeft <= 0));

    }

    #endregion

    public void SetAxis(Vector3 globalFrom, Vector3 globalTo)
    {
        SetAxisDirection(axisPoint: globalFrom, axisDirection: globalTo - globalFrom);
    }

    /// <summary>
    /// Sets the position and the direction of this axis in global space
    /// </summary>
    /// <param name="axisPoint"></param>
    /// <param name="axisDirection"></param>
    public void SetAxisDirection(Vector3 axisPoint, Vector3 axisDirection)
    {
        this.axisPoint = axisPoint;
        this.axisDirection = axisDirection.normalized;
        axisDirectionLocal = transform.InverseTransformDirection(axisDirection);
        axisPointLocal = transform.InverseTransformPoint(axisPoint);
    }

    /// <summary>
    /// When using axisPoint, axisDirection, call this function first, in case this object
    /// was moved.
    /// </summary>
    private void UpdateGlobalParameters()
    {
        if (transform.hasChanged)
        {
            axisDirection = transform.TransformDirection(axisDirectionLocal).normalized;
            axisPoint = transform.TransformPoint(axisPointLocal);
            transform.hasChanged = false;
        }
    }

    /// <summary>
    /// Returns the angle in degrees that "fromPoint" needs to rotate around this axis 
    /// in order to be at the same angle as "toPoint".
    /// </summary>
    /// <param name="fromPoint">Starting position in global space</param>
    /// <param name="toPoint">Final position in global space</param>
    /// <returns>The angle in degrees in [-180, 180]</returns>
    public float GetAngle(Vector3 fromPoint, Vector3 toPoint)
    {

        UpdateGlobalParameters();
        // Set "origin" to be a point on the axis.
        fromPoint -= axisPoint;
        toPoint -= axisPoint;

        // project on the plane perpendicular to the axis
        fromPoint -= Vector3.Dot(fromPoint, axisDirection) * axisDirection;
        toPoint -= Vector3.Dot(toPoint, axisDirection) * axisDirection;

        return Vector3.SignedAngle(fromPoint, toPoint, axisDirection);
    }

    /// <summary>
    /// Rotates this axis around itself by the given angle in degrees.
    /// </summary>
    /// <param name="byAngle">Angle in degrees</param>
    public void RotateAround(float byAngle)
    {
        RotateAround(transform, byAngle);
    }

    /// <summary>
    /// Rotates the given object around this axis by the given angle in degrees.
    /// </summary>
    /// <param name="byAngle">Angle in degrees</param>
    public void RotateAround(Transform objectToRotate, float byAngle)
    {
        UpdateGlobalParameters();
        objectToRotate.RotateAround(axisPoint, axisDirection, byAngle);
    }

}
