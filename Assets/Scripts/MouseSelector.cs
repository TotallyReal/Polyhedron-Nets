using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Nets;

/// <summary>
/// Fires an OnObjectPressed event whenever the mouse presses an object (with collider).
/// </summary>
public class MouseSelector : MonoBehaviour
{

    public static MouseSelector Instance { get; private set; }

    private Nets.PlayerInput input;    
    public event EventHandler<Transform> OnObjectPressed;
    

    void Awake()
    {
        Instance = this;

        input = new Nets.PlayerInput();
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

    private Vector2 MousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public bool MouseRaycast(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(MousePosition());
        return Physics.Raycast(ray, out hit);
    }

    /// <summary>
    /// Checks if the mouse points to an object (with collider). If so returns true and puts the transform of 
    /// the object in the out variable. Otherwise returns false.
    /// </summary>
    /// <param name="mouseObject">The object the mouse points to</param>
    /// <returns>A bool showing if the mouse points to an object</returns>
    public bool MouseRaycastObject(out Transform mouseObject)
    {
        mouseObject = null;

        Ray ray = Camera.main.ScreenPointToRay(MousePosition());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mouseObject = hit.transform;
            return true;
        }
        return false;
    }

    public bool MouseRaycastOfType<T>(out T mouseObject)
    {
        Ray ray = Camera.main.ScreenPointToRay(MousePosition());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.transform.gameObject.TryGetComponent(out mouseObject);
        }

        mouseObject = default;
        return false;
    }

    private void MousePressed(InputAction.CallbackContext obj)
    {
        if (MouseRaycastObject(out Transform mouseObject)){ 
            Debug.Log($"{mouseObject.gameObject.name} pressed");
            OnObjectPressed?.Invoke(this, mouseObject);            
        }
    }


}
