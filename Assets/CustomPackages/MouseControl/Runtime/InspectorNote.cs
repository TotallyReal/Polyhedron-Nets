using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Miscellaneous/README")]
public class InspectorNote : MonoBehaviour
{
    [TextArea(10, 1000)]
    public string Comment = "Information Here.";
}
