using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastSelector : MonoBehaviour
{
    public static RaycastSelector Instance { get; private set; }

    [SerializeField] private bool mouseLogs = true;

    private NetsPlayerInput input;


    void Awake()
    {
        Instance = this;

        mousePosition = DefaultMousePosition;
        input = new NetsPlayerInput();
        input.MouseSelection.Enable();
    }

    private void OnEnable()
    {
        input.MouseSelection.EdgeSelect.performed += MousePressed;
    }

    private void OnDisable()
    {
        input.MouseSelection.EdgeSelect.performed -= MousePressed;
    }

    #region --------------- mouse raycast ---------------

    [SerializeField] private bool mouseRaycastActive = false;
    public delegate Vector2 MousePosition();
    private MousePosition mousePosition;

    public void SetMouseRaycastActive(bool active)
    {
        mouseRaycastActive = active;
    }

    /// <summary>
    /// This event is invoked whenever an object with collider is pressed with the mouse,
    /// as long as the mouseRaycastActive is on.
    /// </summary>
    public event EventHandler<Transform> OnObjectPressed;

    /// <summary>
    /// Return the position of the mouse on screen, used for raycasting.
    /// </summary>
    /// <returns></returns>
    private Vector2 DefaultMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public void ChangeMousePositionForRaycasts(MousePosition mousePosition)
    {
        this.mousePosition = (mousePosition != null) ? mousePosition : DefaultMousePosition;
    }

    private void MousePressed(InputAction.CallbackContext obj)
    {
        if (!mouseRaycastActive)
            return;
        if (ScreenRaycastObject(mousePosition(), out Transform mouseObject))
        {
            if (mouseLogs)
                Debug.Log($"{mouseObject.gameObject.name} pressed");
            OnObjectPressed?.Invoke(this, mouseObject);
        }
    }

    #endregion

    #region --------------- (static) screen raycast ---------------

    public static bool ScreenRaycast(Vector2 screenPoint, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        return Physics.Raycast(ray, out hit);
    }

    /// <summary>
    /// Checks if there is an object with collider at the given screenPoint. If so returns true 
    /// and puts the object's transform in the out variable. Otherwise returns false.
    /// </summary>
    /// <param name="objectTransform">The possible object at the screenPoint position</param>
    /// <returns>A bool showing if there is an objected pointed at</returns>
    public static bool ScreenRaycastObject(Vector2 screenPoint, out Transform objectTransform)
    {
        objectTransform = null;

        if (ScreenRaycast(screenPoint, out RaycastHit hit))
        {
            objectTransform = hit.transform;
            return true;
        }
        return false;
    }

    public static bool ScreenRaycastOfType<T>(Vector2 screenPoint, out T raycaseObject)
    {
        if (ScreenRaycastObject(screenPoint, out Transform objectTransform))
        {
            return objectTransform.gameObject.TryGetComponent<T>(out raycaseObject);
        }

        raycaseObject = default;
        return false;
    }

    #endregion
}
