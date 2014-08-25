using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{

    public Vector3 direction = Vector3.zero;
    public float threshold = 0.2f;

    private SceneScript sceneController;
    public int points = 0;

    int stayCount = 0;

    public int stayThreshold = 0;
    public float wallCollideThreshold = 0;

    float timer = 0;

    float stayTriggerTime = 0;

    // Use this for initialization
    void Start()
    {
        this.sceneController = GameObject.Find("SceneScript").GetComponent<SceneScript>();
        direction = new Vector3(Random.Range(-threshold, threshold), Random.Range(-threshold, threshold));
    }

    public void setSpeed(float speed)
    {
        this.threshold = speed;
    }

    // Update is called once per frame
    void Update()
    {
        print(threshold);
        float tmp = (Time.time - stayTriggerTime);
        if (stayTriggerTime != 0 && tmp > 0.1)
        {
            print("destroy");
            sceneController.addPoints(points);
            Destroy(this.gameObject);
        }

        if (timer <= 0)
        {
            int chance = Random.Range(0, 100);
            if (chance > 90)
            {
                direction = new Vector3(Random.Range(-threshold, threshold), Random.Range(-threshold, threshold));
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
        if (!float.IsNaN(direction.x))
        {
            move();
        }

    }

    private void move()
    {
        this.transform.position += direction;
    }

    public void OnTriggerEnter(Collider collider)
    {
        stayCount = 0;
        hitted();
    }

    void OnTriggerStay(Collider other)
    {
        stayTriggerTime = Time.time;
    }
    void OnTriggerExit(Collider other)
    {
        hitted();
    }

    public void hitted()
    {
        changeDir();
    }

    private void changeDir()
    {
        if (direction == Vector3.zero)
        {
            direction = new Vector3(Random.Range(-threshold, threshold), Random.Range(-threshold, threshold));
        }
        else
        {
            float xDist = Mathf.Abs(this.transform.position.x);
            float yDist = Mathf.Abs(this.transform.position.y);
            float dist = xDist + yDist;

            float xPerc = xDist / dist;
            float yPerc = yDist / dist;
            float x = threshold * xPerc;
            float y = threshold * yPerc;

            if (this.transform.position.x > 0)
            {
                x = -x;
            }
            if (this.transform.position.y > 0)
            {
                y = -y;
            }

            direction = new Vector3(x, y, 0);

            timer = wallCollideThreshold;
        }
    }
}
