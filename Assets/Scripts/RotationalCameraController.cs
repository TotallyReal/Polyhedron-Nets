using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;
using Nets;

/// <summary>
/// Use to control the camera position using polar coordinates
/// </summary>
public class RotationalCameraController : MonoBehaviour
{
    [Header("Camera Position")]
    [Range(-Mathf.PI, Mathf.PI)]
    [SerializeField] private float verticalAngle = 0;
    [SerializeField] private float horizontalAngle = 0;
    [Range(1, 100)]
    [SerializeField] private float dist = 100;

    [Header("Change Parameter")]
    [SerializeField] private float angleChange = 0.01f;
    [SerializeField] private float zoomChange = 2;


    private Vector3 origin = new Vector3(0, 0, 0);

    public void Start()
    {
        StartInput();
        UpdateCartesianCoordinates();
    }

    public void OnValidate()
    {
        UpdateCartesianCoordinates();
    }

    public void RotateRight()
    {
        verticalAngle += angleChange;
        UpdateCartesianCoordinates();
    }

    public void RotateLeft()
    {
        verticalAngle -= angleChange;
        UpdateCartesianCoordinates();
    }

    private bool autoRotate = false;

    private void AutoRotate_performed(InputAction.CallbackContext obj)
    {
        autoRotate = !autoRotate;
    }

    private void UpdateCartesianCoordinates()
    {
        // Keep angle in [-pi/2, pi/2]
        verticalAngle = Mathf.Clamp(verticalAngle, -Mathf.PI / 2, Mathf.PI / 2);
        dist = Mathf.Max(dist, 1);
        transform.position = FromPolar(horizontalAngle, verticalAngle, dist);
        transform.LookAt(origin, FromPolar(horizontalAngle + Mathf.PI, Mathf.PI / 2 - verticalAngle, dist));
    }

    public void Update()
    {        
        Vector2 movement = ReadInputMovementValue();
        verticalAngle += angleChange * movement.y;
        horizontalAngle += angleChange * movement.x;
        if (autoRotate)
            horizontalAngle += angleChange;

        float z = ReadInputZoomValue();
        dist -= z * zoomChange;
        UpdateCartesianCoordinates();

    }

    private Vector3 FromPolar(float phi, float theta, float r)
    {
        return new Vector3(r * Mathf.Cos(phi) * Mathf.Cos(theta), r * Mathf.Sin(theta), r * Mathf.Sin(phi) * Mathf.Cos(theta));
    }

    #region -------------------------- Input --------------------------

    private Nets.PlayerInput input;

    private void StartInput()
    {
        input = new Nets.PlayerInput();
        input.Camera.Enable();
        input.Camera.AutoRotate.performed += AutoRotate_performed;
    }

    private Vector2 ReadInputMovementValue()
    {
        return input.Camera.Movement.ReadValue<Vector2>();
    }

    private float ReadInputZoomValue()
    {
        return input.Camera.Zoom.ReadValue<float>();
    }

    #endregion
}