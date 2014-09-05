using UnityEngine;
using System.Collections;

public class ObjectScroller : MonoBehaviour
{

    private Vector3 startPos;
    public float speed;


    float xSpeed = 0;
    float ySpeed = 0;
    float topxSpeed = 0;
    float topySpeed = 0;

    bool scroll = false;
    // Use this for initialization
    void Start()
    {
        topxSpeed = speed * 0.6f;
        topySpeed = speed * 0.4f;
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (scroll)
        {
            if (xSpeed < topxSpeed)
            {
                xSpeed += topxSpeed / 250;
            }

            if (ySpeed < topySpeed)
            {
                ySpeed += topySpeed / 250;
            }
            Vector3 TreePos = transform.position;

            transform.position = new Vector3(TreePos.x - xSpeed, TreePos.y - ySpeed, TreePos.z);

            if (gameObject.transform.position.y < 1.2f)
            {

                this.transform.position = startPos;

            }
        }
    }

    public void StartScroll()
    {
        this.scroll = true;
    }

    public void StopScroll()
    {
        this.scroll = false;
    }
}
