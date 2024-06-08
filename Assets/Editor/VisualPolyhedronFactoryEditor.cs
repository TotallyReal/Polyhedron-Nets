using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VisualPolyhedronFactory))]
public class VisualPolyhedronFactoryEditor : Editor
{

    public override void OnInspectorGUI()
    {
        VisualPolyhedronFactory factory = (VisualPolyhedronFactory)target;

        if (GUILayout.Button("Build Polyhedron"))
        {
            Debug.Log("Building ...");
            factory.CreatePolyhedron();
        }
        if (GUILayout.Button("Destroy Existing Polyhedron"))
        {
            Debug.Log("Destroying ...");
            factory.DestroyPolyhedron();
        }
        if (GUILayout.Button("Build New Polyhedron"))
        {
            Debug.Log("Building New...");
            factory.CreateNewPolyhedron();
        }
        if (GUILayout.Button("Add vertices"))
        {
            Debug.Log("Adding vertices...");
            factory.AddVertices();
        }
        base.OnInspectorGUI();
    }


}
