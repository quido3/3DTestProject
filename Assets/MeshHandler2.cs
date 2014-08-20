using UnityEngine;
using System.Collections;

public class MeshHandler2 : MonoBehaviour
{

    bool instantiated = false;
    public GameObject meshPref;
    public GameObject meshParent;
    public float riseThreshold = 0.1f;

    private GameObject risingObject;

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
                Vector3 newPos = this.transform.position;
                newPos.z -= riseThreshold;
                risingObject.transform.position = newPos;
            }
            else
            {
                Vector3 newPos = this.transform.position;
                newPos.z = 0;
                meshUp();
                risingObject.transform.position = newPos;
            }
        }
    }

    public void meshUp()
    {
        foreach (GameObject t in meshParent.transform)
        {
            if (t != risingObject)
            {
                Destroy(t);
            }
        }
    }

    public void meshCutted()
    {
        instantiated = true;
        risingObject = (GameObject)Instantiate(meshPref, new Vector3(0, 0, 4), Quaternion.identity);
        risingObject.transform.parent = meshParent.transform;
    }

}
