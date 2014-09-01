using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopSceneSript : MonoBehaviour
{

    int newScore = 0;
    int newLevel = 0;

    Vector2 startSpot;
    Vector2 addAmount;
    float textSize;

    List<int> scoreList;

    public Font font;

    float titleSize;

    // Use this for initialization
    void Start()
    {
        newScore = PlayerPrefs.GetInt(SS.Points);
        print(newScore);
        scoreList = new List<int>();
        bool added = false;
        for (int i = 1; i <= 10; i++)
        {
            int score = PlayerPrefs.GetInt("score" + i, 9);
            if (score < newScore && added == false)
            {
                print("add score");
                added = true;
                scoreList.Add(newScore);
            }
            scoreList.Add(score);
        }

        for (int i = 0; i < 10; i++)
        {
            print(scoreList[i]);
            PlayerPrefs.SetInt("score" + (i + 1), scoreList[i]);
        }


    }

    void OnGUI()
    {
        titleSize = Screen.height / 7;
        textSize = (Screen.height - titleSize) / 12;
        addAmount = new Vector2(0, textSize);
        startSpot = new Vector2(Screen.width / 1.7f, titleSize + addAmount.y);
        GUIStyle style = new GUIStyle();
        style.fontSize = Mathf.FloorToInt(textSize);
        style.font = font;
        GUIStyle titleStyle = new GUIStyle(style);
        titleStyle.fontSize = Mathf.FloorToInt(titleSize);
        titleStyle.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, Screen.height / 20, Screen.width, titleSize), "TULOKSET", titleStyle);
        int title2Size = Screen.height / 10;
        titleStyle.fontSize = title2Size;
        GUI.Label(new Rect(0, startSpot.y, Screen.width / 2, title2Size), "TULOKSESI", titleStyle);
        int newScoreSize = Screen.height / 8;
        titleStyle.fontSize = newScoreSize;
        GUI.Label(new Rect(0, startSpot.y + title2Size, Screen.width / 2, newScoreSize), "" + newScore, titleStyle);
        for (int i = 0; i < 10; i++)
        {
            float spotX = startSpot.x;
            float spotY = startSpot.y + (addAmount.y * i);
            GUI.Label(new Rect(spotX, spotY, Screen.width, textSize), i + 1 + ". " + scoreList[i], style);
        }
    }
}
