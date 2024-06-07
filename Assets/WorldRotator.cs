using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldRotator : MonoBehaviour
{

    [SerializeField] private float rotationTimeSec = 1f;
    private bool isRotating;
    private NetsPlayerInput input;

    void Awake()
    {
        input = new NetsPlayerInput();
        input.Player.Enable();
    }

    private void OnEnable()
    {
        input.Player.RotateX.performed += RotateX;
        input.Player.RotateY.performed += RotateY;
        input.Player.RotateZ.performed += RotateZ;
    }

    private void OnDisable()
    {
        input.Player.RotateX.performed -= RotateX;
        input.Player.RotateY.performed -= RotateY;
        input.Player.RotateZ.performed -= RotateZ;
    }

    private void FinishedRotating()
    {
        isRotating = false;
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
