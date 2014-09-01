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
        spawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnEnemies()
    {
        float speed = 0.02f + (PlayerPrefs.GetInt(SS.Level) * 0.002f);
        print("spawning enemies");
        GameObject g;
        int amount = 5 + (PlayerPrefs.GetInt(SS.Level) * 2);
        print(amount);
        for (int i = 0; i < amount; i++)
        {
            float big = Random.Range(0, 100);
            if (big > 80)
            {
                g = (GameObject)SS.MyInstantiate(enemyBigPref, Vector3.zero, enemyContainer);
                g.GetComponent<EnemyScript>().setSpeed(speed / 2);
            }
            else
            {
                g = (GameObject)SS.MyInstantiate(enemyPref, Vector3.zero, enemyContainer);
                g.GetComponent<EnemyScript>().setSpeed(speed);
            }

        }
    }
}
