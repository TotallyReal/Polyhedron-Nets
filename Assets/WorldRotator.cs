using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class WorldRotator : MonoBehaviour
{

    [SerializeField] private Transform cube;

    [SerializeField] private float rotationTimeSec = 1f;
    private bool isRotating;
    private NetsPlayerInput input;

    struct RotationOp
    {
        public RotationOp(Vector3 axis, float angle)
        {
            normalizedAxis = axis.normalized;
            this.angle = angle;
        }

        public Vector3 normalizedAxis { get; }
        public float angle { get; }

        public override string ToString() => $"{normalizedAxis}[{angle}]";
    }


    private List<RotationOp> axes;

    void Awake()
    {
        AbstractGroupPolyhedron groupPoly = AbstractGroupPolyhedron.Cube(1);
        axes = new List<RotationOp>();
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


        input = new NetsPlayerInput();
        input.Player.Enable();
    }

    private void OnEnable()
    {
        input.Player.RotateX.performed += RotateX;
        input.Player.RotateY.performed += RotateY;
        input.Player.RotateZ.performed += RotateZ;

        RaycastSelector.Instance.OnObjectPressedPlus += Rotate;
    }

    private void OnDisable()
    {
        input.Player.RotateX.performed -= RotateX;
        input.Player.RotateY.performed -= RotateY;
        input.Player.RotateZ.performed -= RotateZ;

        RaycastSelector.Instance.OnObjectPressedPlus -= Rotate;
    }

    private void Rotate(object sender, (Transform, RaycastHit) e)
    {
        if (e.Item1 == cube)
        {
            Vector3 dir = e.Item2.point - transform.position;
            dir.Normalize();
            float dist = 10; // > 2
            RotationOp closestRotOp = new RotationOp(Vector3.up, 0);
            foreach (RotationOp currentRotOp in axes) {
                float d = Vector3.Distance(dir, currentRotOp.normalizedAxis);
                if (d<dist)
                {
                    closestRotOp = currentRotOp;
                    dist = d;
                }
            }

            Debug.Log(dir);
            Debug.Log(closestRotOp.normalizedAxis);
            RotateAround(closestRotOp.normalizedAxis, closestRotOp.angle, 1);
        }
    }

    private void FinishedRotating()
    {
        isRotating = false;
    }

    private void RotateAround(Vector3 axis, float angle, float rotationTimeSec)
    {
        if (isRotating) // TODO: Consider saving the action, and run it once the current action finishes.
        {
            return;
        }

        Quaternion targetRotation = Quaternion.AngleAxis(angle, axis) * transform.rotation;

        isRotating = true;
        transform.DORotateQuaternion(targetRotation, rotationTimeSec)
            .SetRelative(false)
            .OnComplete(FinishedRotating);        
    }

    private void RotateX(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isRotating) // TODO: Consider saving the action, and run it once the current action finishes.
        {
            return;
        }
        isRotating = true;
        transform.DORotate(new Vector3(90, 0, 0), rotationTimeSec, RotateMode.WorldAxisAdd)
            .OnComplete(FinishedRotating);
        //transform.Rotate(90, 0, 0, Space.World);
    }

    private void RotateY(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isRotating)
        {
            return;
        }
        isRotating = true;
        transform.DORotate(new Vector3(0, 90, 0), rotationTimeSec, RotateMode.WorldAxisAdd)
            .OnComplete(FinishedRotating);
        //transform.Rotate(0, 90, 0, Space.World);
    }

    private void RotateZ(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isRotating)
        {
            return;
        }
        isRotating = true;
        transform.DORotate(new Vector3(0, 0, 90), rotationTimeSec, RotateMode.WorldAxisAdd)
            .OnComplete(FinishedRotating);
        //transform.Rotate(0, 0, 90, Space.World);
    }

}
