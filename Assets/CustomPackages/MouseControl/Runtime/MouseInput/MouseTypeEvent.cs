using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Cover the behavior type of a mouse, namely screenPosition on screen and button press.
 * Coupled with basic methods for finding the object pressed.
 */
[DefaultExecutionOrder(-2)]
public class MouseTypeEvent: MonoBehaviour
{ 

    public enum Dimension
    {
        D3,
        D2
    }

    [SerializeField] private string mouseInputName;
    [SerializeField] private Dimension dimension;
    [Tooltip("An event which fires each time the 'mouse' is pressed")]
    [SerializeField] private InputActionReference mouseSelectReference;
    [Tooltip("An event containing the 'mouse' position")]
    [SerializeField] private InputActionReference mousePositionReference;
    public InputAction MouseSelect { get; private set; }
    public InputAction MousePosition { get; private set; }


    private bool mouseRaycastActive = true;
    private bool mouseLogs = true;

    private void Awake()
    {
        MouseSelect = mouseSelectReference.action;
        MousePosition = mousePositionReference.action;
    }

    
    public event EventHandler<Transform> OnObjectSelect;
    public event EventHandler<(Transform, Vector3)> OnObjectSelectPlus;

    public delegate Vector2 GetPosition();

    /**
     * mouseSelect should be fired when the "mouse" button is pressed, and mousePosition
     */
    
    #region ------------------------ positions and objects ------------------------
    public Vector2 ScreenPosition()
    {
        return MousePosition.ReadValue<Vector2>();
    }

    public Vector3 WorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(ScreenPosition());
    }

    public Transform ObjectAtMousePosition()
    {
        return ObjectAtScreenPosition(ScreenPosition(), dimension, out Vector3 hitPosition);
    }

    public (Transform obj, Vector3 position) HitAtMousePosition()
    {
        Transform obj = ObjectAtScreenPosition(ScreenPosition(), dimension, out Vector3 hitPosition);
        return (obj, hitPosition);
    }

    public static Transform ObjectAtScreenPosition(
        Vector2 screenPoint, Dimension dimension, out Vector3 hitPosition)
    {
        hitPosition = Vector3.zero;
        if (dimension == Dimension.D3)
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out RaycastHit rayHit))
                return null;
            hitPosition = rayHit.point;

            // TODO: read about GetRayIntersectionNonAlloc

            if (!rayHit.collider)
                return null;
            return rayHit.transform;
        } else
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(screenPoint));
            if (rayHit.collider == null)
                return null;
            hitPosition = Camera.main.ScreenToWorldPoint(screenPoint);
            return rayHit.transform;
        }
        
        
    }

    #endregion

    #region ------------------------ events ------------------------

    public void SetMouseRaycastActive(bool active)
    {
        mouseRaycastActive = active;
    }

    private void OnEnable()
    {
        MouseSelect.Enable();
        MousePosition.Enable();
        MouseSelect.performed += MousePressed;
    }

    private void OnDisable()
    {
        MouseSelect.performed -= MousePressed;
    }

    private void MousePressed(InputAction.CallbackContext obj)
    {
        if (!mouseRaycastActive)
            return;

        Vector2 screenPosition = ScreenPosition();
        Transform objPressed = ObjectAtScreenPosition(screenPosition, dimension, out Vector3 hitPosition);
        if (objPressed != null)
        {
            if (mouseLogs)
                Debug.Log($"{mouseInputName}: {objPressed.gameObject.name} pressed at screenPosition {screenPosition}");
            OnObjectSelect?.Invoke(this, objPressed);
            OnObjectSelectPlus?.Invoke(this, (objPressed, hitPosition));
        }
    }
    
    #endregion
}
