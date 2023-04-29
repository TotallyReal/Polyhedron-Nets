using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An object which invokes a positionChanged event whenever its position changes.
/// </summary>
[ExecuteInEditMode]
public class ControlVector3 : MonoBehaviour
{

    public UnityEvent<Vector3> positionChanged;

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            positionChanged.Invoke(transform.position);
        }
    }
}
