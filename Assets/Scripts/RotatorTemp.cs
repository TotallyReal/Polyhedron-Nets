using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorTemp : MonoBehaviour
{

    public Transform t1;
    public Transform t2;

    public bool b = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnValidate()
    {
        if (!b)
            return;
        Vector3 v1 = t1.position;
        Vector3 v2 = t2.position;
        transform.position = Vector3.Lerp(v1, v2, 0.5f);
        Vector3 dir = v2 - v1;
        Vector3 rotationAxis = Vector3.Cross(dir, transform.up);
        rotationAxis.Normalize();
        float angle = Vector3.SignedAngle(transform.up, dir, rotationAxis);
        transform.Rotate(transform.InverseTransformDirection(rotationAxis), angle);

        transform.localScale = new Vector3(0.5f, dir.magnitude / 2, 0.5f);

    }
}
