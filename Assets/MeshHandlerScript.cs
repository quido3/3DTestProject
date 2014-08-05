using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshHandlerScript : MonoBehaviour
{
    public GameObject firstMesh;

    Vector3 originalPos = Vector3.zero;
    List<GameObject> meshList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        meshList.Add(firstMesh);
        originalPos = firstMesh.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        foreach (GameObject go in meshList)
        {
            Vector3 pos = go.transform.position;
            Vector3 tmpPos = pos;
            if (pos.z > originalPos.z)
            {
                pos.z -= 1 * Time.deltaTime;
            }
            else if (tmpPos.z != originalPos.z)
            {
                pos.z = originalPos.z;
                reconnectPiece(go);
            }
            go.transform.position = pos;
        }
    }

    private void reconnectPiece(GameObject go)
    {
        List<GameObject> adjacentPieces = new List<GameObject>();
        Mesh m = go.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] verts = m.vertices;
        print("connecting");
        foreach (Vector3 v in verts)
        {
            Vector3 direction = v;
            direction.z = 1;
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(v, direction, out hit))
            {
                adjacentPieces.Add(hit.transform.gameObject);
            }
        }

        foreach (GameObject g in adjacentPieces)
        {
            mergeMeshes(go, g);
        }
    }

    private void mergeMeshes(GameObject g1, GameObject g2)
    {
        Vector3[] g1Verts = g1.GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3[] g2Verts = g2.GetComponent<MeshFilter>().sharedMesh.vertices;

        List<Vector3> vertsToDelete = new List<Vector3>();

        List<Vector3> newVerts = new List<Vector3>();

        foreach (Vector3 g1V in g1Verts)
        {
            foreach (Vector3 g2V in g2Verts)
            {
                if (g2V == g1V)
                {
                    vertsToDelete.Add(g1V);
                }
            }
        }

        foreach (Vector3 v in g1Verts)
        {
            bool toBeLeft = false;
            foreach (Vector3 deleteVert in vertsToDelete)
            {
                if (v == deleteVert)
                {
                    toBeLeft = true;
                }
            }
            if (toBeLeft == false)
            {
                newVerts.Add(v);
            }
        }

        foreach (Vector3 v in g2Verts)
        {
            bool toBeLeft = false;
            foreach (Vector3 deleteVert in vertsToDelete)
            {
                if (v == deleteVert)
                {
                    toBeLeft = true;
                }
            }
            if (toBeLeft == false)
            {
                newVerts.Add(v);
            }
        }

        //That part need more work. UVS and nothing have been set at all. The vertices are added to same array, but they can so that first is at top and last at bottom. Then the last triangle will be set to shit.

    }

    public void addMesh(GameObject newMesh)
    {
        meshList.Add(newMesh);
    }
}
