using UnityEngine;
using System.Collections;

public class MeshHandler2 : MonoBehaviour
{
    public GameObject meshPref;
    public GameObject meshParent;
    public float riseThreshold = 0.1f;

    public GameObject upObject;

    public GameObject risingObject;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (risingObject != null)
        {
            if (risingObject.transform.position.z > 0)
            {
                Vector3 newPos = risingObject.transform.position;
                newPos.z -= riseThreshold;
                risingObject.transform.position = newPos;
            }
            else
            {
                Vector3 newPos = risingObject.transform.position;
                newPos.z = 0;
                risingObject.transform.position = newPos;
                meshUp();
            }
        }
    }

    public void meshUp()
    {
        GameObject toDestroy = null;
        foreach (Transform t in meshParent.transform)
        {
            if (t.gameObject != risingObject)
            {
                toDestroy = t.gameObject;
            }
        }

        

        upObject = risingObject;
        upObject.GetComponent<MeshCollider>().enabled = true;
        changeMaterialColor(upObject, Color.white);
        
        risingObject = null;
        Destroy(toDestroy);
    }

    public void meshCutted()
    {
        if (risingObject == null)
        {
            risingObject = (GameObject)Instantiate(meshPref, new Vector3(0, 0, 4), Quaternion.identity);
            risingObject.transform.parent = meshParent.transform;
            risingObject.GetComponent<MeshCollider>().enabled = false;
            changeMaterialColor(risingObject, Color.grey);
        }
    }

    private void changeMaterialColor(GameObject g, Color color)
    {
        foreach (Material mat in g.GetComponent<MeshRenderer>().materials)
        {
            mat.SetColor("_Color", color);
        }
    }

    public GameObject getSliceObject()
    {
        return upObject;
    }

}
