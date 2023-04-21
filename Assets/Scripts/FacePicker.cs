using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FacePicker : MonoBehaviour
{

    [SerializeField] private VisualPolyhedronFactory polyhedronFactory;

    private void Start()
    {
        RaycastSelector.Instance.OnObjectPressed += OnObjectPressed;
    }

    private void OnObjectPressed(object sender, Transform e)
    {
        if (e.TryGetComponent<FaceMesh>(out FaceMesh face)){
            polyhedronFactory.AddCube(face);
        }
    }
}
