using UnityEngine;
using System.Collections;

public class ScrollHandler : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startScrolls()
    {
        foreach (Transform child in this.transform)
        {
            ObjectScroller scroller = child.GetComponent<ObjectScroller>();
            if (scroller != null)
            {
                scroller.StartScroll();
            }
        }
    }

    public void stopScrolls()
    {
        foreach (Transform child in this.transform)
        {
            ObjectScroller scroller = child.GetComponent<ObjectScroller>();
            if (scroller != null)
            {
                scroller.StopScroll();
            }
        }
    }
}
