using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(mouseSlicer))]
public class customInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        mouseSlicer myScript = (mouseSlicer)target;
        if (GUILayout.Button("resetTrail"))
        {

        }
    }
}