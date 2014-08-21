using UnityEngine;
using System.Collections;

public class EnemyHandlerScript : MonoBehaviour
{

    public GameObject enemyPref;
    public GameObject enemyBigPref;

    public GameObject enemyContainer;

    public int enemyCount;

    // Use this for initialization
    void Start()
    {
        spawnEnemies(enemyCount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnEnemies(int amount)
    {
        print("spawning enemies");
        for (int i = 0; i < amount; i++)
        {
            float big = Random.Range(0, 100);
            if (big > 80)
            {
                SS.MyInstantiate(enemyBigPref, Vector3.zero, enemyContainer);
            }
            SS.MyInstantiate(enemyPref, Vector3.zero, enemyContainer);
        }
    }
}
