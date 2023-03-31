using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EdgePicker : MonoBehaviour
{

    private MouseTarget selectedTarget = null;

    private void Start()
    {
        MouseSelector.Instance.OnObjectPressed += OnObjectPressed;
    }

    /*private void OnEnable()
    {
        MouseSelector.Instance.OnObjectPressed += OnObjectPressed;
    }

    private void OnDisable()
    {
        MouseSelector.Instance.OnObjectPressed -= OnObjectPressed;
    }*/

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
