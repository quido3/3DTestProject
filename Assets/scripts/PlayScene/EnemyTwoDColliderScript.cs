using UnityEngine;
using System.Collections;

public class EnemyTwoDColliderScript : MonoBehaviour
{

    EnemyScript parent;

    // Use this for initialization
    void Start()
    {
        parent = this.transform.parent.GetComponent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        //print("trigger enter");
        parent.hitted();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //print("trigger stay");
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //print("trigger exit");
    }
}
