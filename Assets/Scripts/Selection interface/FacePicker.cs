using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: When I go back to the general cube polyhedron construction, improve this file.
public class FacePicker : MonoBehaviour
{

    [SerializeField] private VisualPolyhedronFactory polyhedronFactory;
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
        if (e.TryGetComponent<FaceMesh>(out FaceMesh face)){
            polyhedronFactory.AddCube(face);
        }
    }
}
