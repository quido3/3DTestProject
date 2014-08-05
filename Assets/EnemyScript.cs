using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
    public List<GameObject> objects;
    public GameObject plane;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {




        /*Vector3[] verts = plane.GetComponent<MeshFilter>().sharedMesh.vertices;
        for (int i = 0; i < 30; i++)
        {
            Debug.DrawLine(Vector3.zero, this.transform.position);
            Vector3[] points = new Vector3[4];
            for (int o = 0; o < 4; o++)
            {
                points[o] = this.transform.position;
            }
            if (Poly.ContainsPoint(verts, points))
            {
                print("inside");
            }
            else
            {
                print("out");
            }
        }*/
    }
}
