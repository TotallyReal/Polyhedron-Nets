using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastSelector : MonoBehaviour
{
    /**
     * Remember to move this class ahead in the script execution order (in project settings).
     */


    public static RaycastSelector Instance { get; private set; }

    /// <summary>
    /// Used for different kind of mouse events types, e.g. left mouse, right mouse, left+control, touch, etc.
    /// It has a mouse press event, and a position on screen.
    /// </summary>
    public class MouseEvent
    {

        private bool mouseRaycastActive = true;
        private bool mouseLogs = true;
        private string name;

        public InputAction MouseSelect;
        public InputAction MousePosition;

        public event EventHandler<Transform> OnObjectSelect;
        public event EventHandler<(Transform, RaycastHit)> OnObjectSelectPlus;

        public MouseEvent(string name, InputAction mouseSelect, InputAction mousePosition)
        {
            this.name = name;
            MouseSelect = mouseSelect;
            MousePosition = mousePosition;
        }

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
            return RaycastSelector.ObjectAtScreenPosition(ScreenPosition(), out RaycastHit rayHit);
        }

        #endregion

        #region ------------------------ events ------------------------

        public void SetMouseRaycastActive(bool active)
        {
            mouseRaycastActive = active;
        }

        internal void OnEnable()
        {
            MouseSelect.performed += MousePressed;
        }

        internal void OnDisable()
        {
            MouseSelect.performed -= MousePressed;
        }

        private void MousePressed(InputAction.CallbackContext obj)
        {
            if (!mouseRaycastActive)
                return;

            Vector2 position = ScreenPosition();
            Transform objPressed = ObjectAtScreenPosition(position, out RaycastHit rayHit);
            if (objPressed != null)
            {
                if (mouseLogs)
                    Debug.Log($"{name}: {objPressed.gameObject.name} pressed at position {position}");
                OnObjectSelect?.Invoke(this, objPressed);
                OnObjectSelectPlus?.Invoke(this, (objPressed, rayHit));
            }
        }

        #endregion
    }


    public static Transform ObjectAtScreenPosition(Vector2 screenPoint, out RaycastHit rayHit)
    {
        //RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(screenPoint));
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out rayHit))
            return null;

        // TODO: read about GetRayIntersectionNonAlloc

        if (!rayHit.collider)
            return null;
        return rayHit.transform;
    }


    // TODO: should not be public
    public static MouseEvent playerMouseEvent;


    private MouseInput input;

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;

        input = new MouseInput();
        input.Player.Enable();

        playerMouseEvent = new MouseEvent(
            "Player", input.Player.PointerSelect, input.Player.PointerPosition);
    }

    private void OnEnable()
    {
        playerMouseEvent.OnEnable();
    }

    private void OnDisable()
    {
        playerMouseEvent.OnEnable();
    }
}
