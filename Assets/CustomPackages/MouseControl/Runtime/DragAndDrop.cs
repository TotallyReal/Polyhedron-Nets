using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should have a collider
public class DragAndDrop : MonoBehaviour
{

    public bool dragIsActive { get; set; } = true;
    [Tooltip("Leave empty to use the current object")]
    [SerializeField] private Transform draggedObject;
    [SerializeField] private MouseTypeEvent mouseEvent;

    private bool isDragging = false;
    
    private void Awake()
    {
        mouseEvent = (mouseEvent == null) ? DefaultMouseTypeEvent.standard : mouseEvent;
        draggedObject = (draggedObject == null) ? transform : draggedObject;
    }

    public void SetMouseEvent(MouseTypeEvent mouseEvent)
    {
        if (mouseEvent != null)
        {
            OnDisable();
            this.mouseEvent = mouseEvent;
            OnEnable();
        }
    }

    private void OnEnable()
    {
        mouseEvent.OnObjectSelectPlus += OnObjectPressed;
        mouseEvent.MouseSelect.canceled += MouseSelect_canceled;
    }

    private void OnDisable()
    {
        mouseEvent.OnObjectSelectPlus -= OnObjectPressed;
        mouseEvent.MouseSelect.canceled -= MouseSelect_canceled;
    }

    public event EventHandler<Vector3> OnPressed;
    public event EventHandler<Vector3> OnStartDragging;
    public event EventHandler<Vector3> OnDragging;
    public event EventHandler<Vector3> OnEndDragging;

    private void OnObjectPressed(object sender, (Transform obj, Vector3 position) arg)
    {
        if (arg.obj == draggedObject)
        {
            OnPressed?.Invoke(this, arg.position); // TODO: should I keep this event?
            if (dragIsActive)
            {
                isDragging = true;
                OnStartDragging?.Invoke(this, arg.position);
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            (Transform obj, Vector3 position) = mouseEvent.HitAtMousePosition();
            if (obj != draggedObject)
            {
                return; 
                // TODO: For now "continue" dragging even if not touching the object.
                //       Stops when mouse button is released. Maybe fire an event for "out of bounds" dragging?
            }
            OnDragging?.Invoke(this, position);
        }
    }

    private void MouseSelect_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isDragging)
        {
            isDragging = false;
            OnEndDragging?.Invoke(this, mouseEvent.HitAtMousePosition().position);
        }
    }
}