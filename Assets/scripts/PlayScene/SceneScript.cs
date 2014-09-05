using UnityEngine;
using System.Collections;

public class SceneScript : MonoBehaviour
{

    private int currentLevel = 0;
    private int currentPoints = 0;

    public TextMesh points;
    public TextMesh level;

    // Use this for initialization
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt(SS.Level, 1);
        currentPoints = PlayerPrefs.GetInt(SS.Points, 0);
        points.text = "" + currentPoints;
        level.text = "" + currentLevel;
    }

    public void addPoints(int points)
    {
        this.currentPoints += points;
        this.points.text = "" + currentPoints;
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = 60;
    }

    public void EndGame()
    {
        print("Level fail");
        SavePrefs();
        Application.LoadLevel(2);
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetInt(SS.Level, currentLevel);
        PlayerPrefs.SetInt(SS.Points, currentPoints);
    }

    public void levelClear()
    {
        print("Level clear");
        currentLevel++;
        SavePrefs();
        Application.LoadLevel(0);
    }
}
