using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PolyhedronKeyboardInput : MonoBehaviour
{

    /// <summary>
    /// Invoked after an unfolding operation. The parameter indicates if 
    /// the polyhedron is fully unfolded.
    /// </summary>
    public event EventHandler<bool> OnPolyhedronUnfolded;
    public event EventHandler OnRestart;

    private VisualPolyhedronFactory visualPolyhedronFactory;
    

    // Start is called before the first frame update
    void Awake()
    {
        visualPolyhedronFactory = GetComponent<VisualPolyhedronFactory>();
    }

    private void Start()
    {
        NetsPlayerInput input = new NetsPlayerInput();
        input.Player.Enable();

        input.Player.Unfold.performed += CreateGraph;
        input.Player.Restart.performed += RestartPolyhedron;
    }

    private void RestartPolyhedron(InputAction.CallbackContext obj)
    {
        OnRestart?.Invoke(this, EventArgs.Empty);
        EscapeRoomManager.Instance.RestartPolyhedrons();
    }

    private void CreateGraph(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        VisualPolyhedron visualPolyhedron = visualPolyhedronFactory.GetVisualPolyhedron();
        if (visualPolyhedron != null)
        {
            FaceGraph faceGraph = visualPolyhedron.GetComponent<FaceGraph>();
            faceGraph.CreateTransformGraph();
            OnPolyhedronUnfolded?.Invoke(this, faceGraph.IsTree());
        }
    }
}
