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
            Undo.RecordObject(factory, "building visual polyhedron");
            factory.CreatePolyhedron();
            EditorUtility.SetDirty(factory);
            serializedObject.Update();
        }
        if (GUILayout.Button("Destroy Existing Polyhedron"))
        {
            Debug.Log("Destroying ...");
            Undo.RecordObject(factory, "Destroying visual polyhedron");
            factory.CreatePolyhedron();
            factory.DestroyPolyhedron();
            serializedObject.Update();
        }
        if (GUILayout.Button("Build New Polyhedron"))
        {
            Debug.Log("Building New...");
            Undo.RecordObject(factory, "Building New visual polyhedron");
            factory.CreatePolyhedron();
            factory.CreateNewPolyhedron();
            serializedObject.Update();
        }
        if (GUILayout.Button("Add vertices"))
        {
            Debug.Log("Adding vertices...");
            Undo.RecordObject(factory, "Adding vertices");
            factory.CreatePolyhedron();
            factory.AddVertices();
            serializedObject.Update();
        }
        base.OnInspectorGUI();
    }


}
