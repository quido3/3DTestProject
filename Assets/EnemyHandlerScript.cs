using UnityEngine;
using System.Collections;

public class EnemyHandlerScript : MonoBehaviour
{

    public GameObject enemyPref;

    public GameObject enemyContainer;

    // Use this for initialization
    void Start()
    {
        //GameObject enemy = (GameObject)Instantiate(enemyPref, new Vector3(0, 0, 0), Quaternion.identity);
        //enemy.transform.parent = enemyContainer.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
