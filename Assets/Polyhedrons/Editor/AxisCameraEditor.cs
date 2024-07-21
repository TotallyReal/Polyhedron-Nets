using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AxisCamera))]
public class VisualPolyhedronFactoryEditor : Editor
{

    public override void OnInspectorGUI()
    {
        AxisCamera factory = (AxisCamera)target;

        if (GUILayout.Button("Set position from transform"))
        {
            Debug.Log("Setting position of axis camera from camera position");
            factory.SetPositionFromTransform();
        }
        base.OnInspectorGUI();
    }


}
