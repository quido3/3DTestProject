using UnityEngine;
using System.Collections;

public class MenuSceneScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        PlayerPrefs.SetInt(SS.Level, 2);
        PlayerPrefs.SetInt(SS.Points, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
