using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attach to a camera focused on a single point, where the player input is used to 
/// rotate around it and zoom in\out.
/// </summary>
public class AxisCamera : MonoBehaviour
{

    [SerializeField] private GameObject lookAtTarget;

    [Header("Movement speed")]
    [SerializeField] private float horizontalSpeed = 1f;
    [SerializeField] private float verticalSpeed = 1f;
    [SerializeField] private float radialSpeed = 1f;

    [Header("Starting position")]
    [SerializeField] private float horAngle = 0;
    [SerializeField] private float verAngle = 0;
    [SerializeField] private float radius = 1;

    private float maxVerAngle = Mathf.PI / 2 - 0.01f;
    private float minVerAngle = 0.01f - Mathf.PI / 2;

    private float minRadius = 1f;
    private float maxRadius = 15f;

    private NetsPlayerInput input;

    private void Awake()
    {
        input = new NetsPlayerInput();
        input.Camera.Enable();

        SetPositionFromTransform(); // TODO: maybe remove this line?
    }

    private void SetPositionFromTransform()
    {
        Vector3 lookAtDir = transform.position - lookAtTarget.transform.position;

        radius = lookAtDir.magnitude;

        horAngle = Mathf.Atan2(lookAtDir.z, lookAtDir.x);
        float xzLength = Mathf.Sqrt(lookAtDir.z * lookAtDir.z + lookAtDir.x * lookAtDir.x);
        verAngle = Mathf.Atan2(lookAtDir.y, xzLength);
    }

    #region ------------------- enable \ disable -------------------

    private void OnEnable()
    {
        input.Camera.MouseZoom.performed += MouseZoom_performed;
    }

    private void OnDisable()
    {
        input.Camera.MouseZoom.performed -= MouseZoom_performed;
    }

    #endregion

    private void MouseZoom_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        float zoomValue = obj.ReadValue<float>()/120f;
        UpdatePolarPosition(0, 0, -zoomValue * radialSpeed);
    }

    private void FixedUpdate()
    {
        Vector2 movement = input.Camera.Movement.ReadValue<Vector2>();
        float zoom = -input.Camera.Zoom.ReadValue<float>();
        UpdatePolarPosition(
            Time.deltaTime * movement.x * horizontalSpeed,
            Time.deltaTime * movement.y * verticalSpeed,
            Time.deltaTime * zoom * radialSpeed);
    }

    public void UpdatePolarPosition(float horChange, float vertChange, float radiusChange)
    {
        radius += radiusChange;
        radius = Mathf.Clamp(radius, minRadius, maxRadius);
        horAngle += horChange;
        verAngle += vertChange;
        verAngle = Mathf.Clamp(verAngle, minVerAngle, maxVerAngle);

        transform.position = lookAtTarget.transform.position + new Vector3(
            radius * Mathf.Cos(verAngle) * Mathf.Cos(horAngle),
            radius * Mathf.Sin(verAngle),
            radius * Mathf.Cos(verAngle) * Mathf.Sin(horAngle));

        transform.LookAt(lookAtTarget.transform);
    }

}
