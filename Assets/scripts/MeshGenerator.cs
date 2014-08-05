using UnityEngine;
using System.Collections;

public class MeshGenerator : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {
        Mesh m = ProceduralGeometry.CreateCircle(1f);
        this.GetComponent<MeshFilter>().sharedMesh = m;
        DestroyImmediate(collider);
        gameObject.AddComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        print("mesh clicked");
    }
}
