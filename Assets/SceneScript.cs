using UnityEngine;
using System.Collections;

public class SceneScript : MonoBehaviour
{

    private int currentLevel = 0;
    private int currectPoints = 0;

    // Use this for initialization
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt(SS.Level);
        currectPoints = PlayerPrefs.GetInt(SS.Points);
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = 60;
    }
}
