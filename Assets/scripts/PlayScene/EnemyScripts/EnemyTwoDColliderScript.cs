using UnityEngine;
using System.Collections;

public class EnemyTwoDColliderScript : MonoBehaviour
{

    EnemyScript parent;

    void Awake()
    {
        parent = this.transform.parent.GetComponent<EnemyScript>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        parent.hitted();
    }

    void OnTriggerStay2D(Collider2D other)
    {
    }
    void OnTriggerExit2D(Collider2D other)
    {
    }
}
