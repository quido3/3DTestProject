using UnityEngine;
using System.Collections;

public class TopSceneSript : MonoBehaviour
{

    int newScore = 0;
    int newLevel = 0;

    // Use this for initialization
    void Start()
    {
        newScore = PlayerPrefs.GetInt(SS.Level);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
