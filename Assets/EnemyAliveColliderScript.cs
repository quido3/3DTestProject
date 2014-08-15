using UnityEngine;
using System.Collections;

public class EnemyAliveColliderScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider collider)
    {
        print("enter trigger");
    }

    void OnTriggerStay(Collider other)
    {
        print("stay trigger");
    }
    void OnTriggerExit(Collider other)
    {
        print("exit trigger");
    }
}
