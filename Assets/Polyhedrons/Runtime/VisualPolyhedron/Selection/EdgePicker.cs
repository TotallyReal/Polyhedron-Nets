using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EdgePicker : MonoBehaviour
{

    private MouseTarget selectedTarget = null;
    private RaycastSelector.MouseEvent mouseEvent = RaycastSelector.playerMouseEvent;


    private void OnEnable()
    {
        mouseEvent.OnObjectSelect += OnObjectPressed;
    }

    private void OnDisable()
    {
        mouseEvent.OnObjectSelect -= OnObjectPressed;
    }

    private void OnObjectPressed(object sender, Transform e)
    {
        if (e.TryGetComponent<MouseTarget>(out MouseTarget target)){
            if (target != selectedTarget && selectedTarget != null)
            {
                selectedTarget.DeselectTarget();
            }
            if (target != null)
                target.SelectTarget();
            selectedTarget = target;
        }
    }
}
