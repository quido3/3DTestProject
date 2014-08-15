using UnityEngine;
using System.Collections;

public class EnemyHandlerScript : MonoBehaviour
{

    public GameObject enemyPref;

    public GameObject enemyContainer;

    public int enemyCount;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SS.MyInstantiate(enemyPref, Vector3.zero, enemyContainer);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
