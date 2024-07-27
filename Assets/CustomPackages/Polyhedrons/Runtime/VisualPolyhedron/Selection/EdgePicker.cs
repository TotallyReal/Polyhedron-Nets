using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EdgePicker : MonoBehaviour
{

    private MouseTarget selectedTarget = null;
    [SerializeField] private MouseTypeEvent mouseEvent;

    private void Awake()
    {
        mouseEvent = (mouseEvent==null)?DefaultMouseTypeEvent.standard:mouseEvent;
    }


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
