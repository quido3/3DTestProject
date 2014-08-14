using UnityEngine;
using System.Collections;

public class LoadSceneBtn : MonoBehaviour
{

    public bool loadMainMenu = false;
    public bool loadPlayScene = false;

    void OnMouseDown()
    {
        if (loadMainMenu)
        {
            Application.LoadLevel(0);
        }
        if (loadPlayScene)
        {
            Application.LoadLevel(1);
        }
    }

}
