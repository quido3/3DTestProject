using UnityEngine;
using System.Collections;

public class StartButtonScript : MonoBehaviour
{

    public TextureScroller scroller;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        scroller.startTravel();
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        foreach (Transform go in this.transform)
        {
            go.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
