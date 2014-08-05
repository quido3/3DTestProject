using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// =============================================================================
public class ProceduralGeometryMenu : EditorWindow
{

    // -------------------------------------------------------------------------
    static void CreateCircle()
    {

        GameObject circleObj = new GameObject("Circle");
        MeshFilter circleMesh = (MeshFilter)circleObj.AddComponent<MeshFilter>();
        circleObj.AddComponent<MeshRenderer>();

        circleMesh.sharedMesh = ProceduralGeometry.CreateCircle(10.0f);

        AssetDatabase.CreateAsset(circleMesh.sharedMesh, "Assets/circlekissa.asset");
    }
}
