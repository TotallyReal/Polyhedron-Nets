using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using StarterAssets;

public class AxisCamera : MonoBehaviour
{

    //[SerializeField] private Camera mainCamera;
    //[SerializeField] private FirstPersonController controller;

    [SerializeField] private GameObject lookAtTarget;
    [SerializeField] private float horizontalSpeed = 1f;
    [SerializeField] private float verticalSpeed = 1f;
    [SerializeField] private float radialSpeed = 1f;

    [SerializeField] private float horAngle = 0;
    [SerializeField] private float verAngle = 0;
    [SerializeField] private float radius = 1;

    private float maxVerAngle = Mathf.PI / 2 - 0.01f;
    private float minVerAngle = 0.01f - Mathf.PI / 2;

    private float minRadius = 1f;
    private float maxRadius = 15f;

    private NetsPlayerInput input;

    private void Start()
    {
        input = new NetsPlayerInput();
        input.Camera.Enable();

        Vector3 lookAtDir = transform.position - lookAtTarget.transform.position;

        radius = lookAtDir.magnitude;
        
        horAngle = Mathf.Atan2(lookAtDir.z, lookAtDir.x);
        float xzLength = Mathf.Sqrt(lookAtDir.z * lookAtDir.z + lookAtDir.x * lookAtDir.x);
        verAngle = Mathf.Atan2(lookAtDir.y, xzLength);
    }

    private void Update()
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
    /*
    public void TakeControl()
    {
        gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        controller.gameObject.SetActive(false);
    }

    public void ReleaseControl()
    {
        mainCamera.gameObject.SetActive(true);
        gameObject.SetActive(false);
        controller.gameObject.SetActive(true);
    }*/

}
