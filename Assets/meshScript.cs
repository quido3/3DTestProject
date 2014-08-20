using UnityEngine;
using System.Collections;

public class meshScript : MonoBehaviour
{

    public float riseThreshold = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.z > 0)
        {
            print("is down");
            Vector3 newPos = this.transform.position;
            newPos.z -= riseThreshold;
            this.transform.position = newPos;
        }
        else
        {
            Vector3 newPos = this.transform.position;
            newPos.z = 0;
            this.transform.position = newPos;
        }
    }
}
