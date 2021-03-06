﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{

    public Vector3 direction = Vector3.zero;
    public float threshold = 0.2f;

    float timer = 0;

    // Use this for initialization
    void Start()
    {
        direction = new Vector3(Random.Range(-threshold, threshold), Random.Range(-threshold, threshold));
    }

    // Update is called once per frame
    void Update()
    {
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
        move();
    }

    private void move()
    {
        this.transform.position += direction;
    }

    public void OnTriggerEnter(Collider collider)
    {
        //print("trigger enter");
        hitted();
    }

    void OnTriggerStay(Collider other)
    {
        //print("trigger stay");
    }
    void OnTriggerExit(Collider other)
    {
        //print("trigger exit");
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

            timer = 0.2f;
        }
    }
}
