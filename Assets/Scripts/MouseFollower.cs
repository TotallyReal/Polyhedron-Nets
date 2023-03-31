using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (MouseSelector.Instance.MouseRaycast(out RaycastHit hit)){
            transform.position = hit.point;
        }       
    }
}
