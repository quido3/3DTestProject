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
        float speed = 0.02f + (PlayerPrefs.GetInt(SS.Level) * 0.002f);
        print(PlayerPrefs.GetInt(SS.Level));
        print("spawning enemies");
        GameObject g;
        for (int i = 0; i < amount; i++)
        {
            float big = Random.Range(0, 100);
            if (big > 80)
            {
                g = (GameObject)SS.MyInstantiate(enemyBigPref, Vector3.zero, enemyContainer);
                g.GetComponent<EnemyScript>().setSpeed(speed / 2);
            }
            g = (GameObject)SS.MyInstantiate(enemyPref, Vector3.zero, enemyContainer);
            g.GetComponent<EnemyScript>().setSpeed(speed);
        }
    }
}
