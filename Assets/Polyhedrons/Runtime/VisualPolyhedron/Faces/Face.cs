using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public int ID { get; set; }
    public List<PolyhedronEdge> edges;

    virtual public Vector3 Normal()
    {
        return transform.up;
    }

    virtual public Vector3 GetGlobalCenter()
    {
        return transform.position;
    }
}
