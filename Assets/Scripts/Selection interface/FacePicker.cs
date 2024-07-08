using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: When I go back to the general cube polyhedron construction, improve this file.
public class FacePicker : MonoBehaviour
{

    [SerializeField] private VisualPolyhedronFactory polyhedronFactory;


    private void OnEnable()
    {
        RaycastSelector.Instance.OnObjectPressed += OnObjectPressed;
    }

    private void OnDisable()
    {
        RaycastSelector.Instance.OnObjectPressed -= OnObjectPressed;
    }

    private void OnObjectPressed(object sender, Transform e)
    {
        if (e.TryGetComponent<FaceMesh>(out FaceMesh face)){
            polyhedronFactory.AddCube(face);
        }
    }
}
