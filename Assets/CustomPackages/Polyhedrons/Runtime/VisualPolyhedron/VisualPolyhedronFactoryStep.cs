using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class VisualPolyhedronFactoryStep : MonoBehaviour
{

    // When overriding this method, make sure that it is independent of the Awake method, since
    // it can be called during the Editor mode.
    abstract public void FactoryStep(VisualPolyhedron visualPolyhedron);

}
