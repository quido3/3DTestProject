﻿using UnityEngine;
using System.Collections;

public class LoadSceneBtn : MonoBehaviour
{

    public bool loadMainMenu = false;
    public bool loadPlayScene = false;
    public bool loadScoreScreen = false;

    void OnMouseDown()
    {
        if (loadMainMenu)
        {
            Application.LoadLevel(0);
        }
        if (loadPlayScene)
        {
            if (GameObject.Find("GPSTexture").GetComponent<TextureScroller>().travelDone())
            {
                Application.LoadLevel(1);
            }
        }
        if (loadScoreScreen)
        {
            Application.LoadLevel(2);
        }
    }

}
