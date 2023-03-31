using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public List<Axis> edges;

    virtual public Vector3 Normal()
    {
        return transform.up;
    }
}
