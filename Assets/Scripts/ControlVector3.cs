using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ControlVector3 : MonoBehaviour
{

    public UnityEvent<Vector3> positionChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
