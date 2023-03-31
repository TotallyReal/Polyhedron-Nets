using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorArrow : MonoBehaviour
{

    [SerializeField] private Transform arrowHead;
    [SerializeField] private Arc arc;

    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [Range(-360, 360)]
    [SerializeField] private float angleDegrees = 10;
    [Range(0,5)]
    [SerializeField] private float seconds = 1;

    private float angleRemaining = 0;
    private float anglePerSecond = 0;
    private int direction = 0;

    private Transform objectToRotate;

    // Start is called before the first frame update
    void Start()
    {
        GroupSelection.Instance.Register(arrowHead, Rotate);
        GroupSelection.Instance.Register(arc.transform, Rotate);
    }

    private void OnValidate()
    {
        transform.up = rotationAxis;
        arc.SetAngle(angleDegrees * Mathf.PI / 180);
    }

    public void SetObjectToRotate(Transform objTransform)
    {
        objectToRotate = objTransform;
    }

    private void Update()
    {
        if (direction == 0)
            return;

        float rotationAngle = direction * anglePerSecond * Time.deltaTime;
        objectToRotate.Rotate(rotationAxis, rotationAngle);
        angleRemaining -= rotationAngle;
        if (angleRemaining <= 0)
        {
            direction = 0;
            angleRemaining = 0;
            anglePerSecond = 0;
        }
    }

    private void Rotate()
    {
        if (direction == 0 && objectToRotate != null)
        {
            angleRemaining = Mathf.Abs(angleDegrees);
            anglePerSecond = angleRemaining / seconds;
            direction = angleDegrees > 0 ? 1 : -1;
        }
    }

}
