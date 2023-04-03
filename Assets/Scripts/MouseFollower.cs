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

            if (VisualPolyhedronFactory.Instance.NearAxis(hit.point, 0.1f))
            {
                transform.localScale = new Vector3(3, 3, 3);
            } else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }       
    }
}
