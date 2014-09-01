using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SliceTrailer : MonoBehaviour
{

    List<List<Vector3>> trailList = new List<List<Vector3>>();
    List<Vector3> vList = null;

    private float differThroshold = 0.7f;

    private float angleThreshold;

    public void Add(Vector3 v)
    {
        if (vectorsDiffer(v, vList[vList.Count - 1]))
        {


            if (!goodAngle(v))
            {
                endTrail(v);
                startTrail(v);
            }
            else
            {
                addToList(v);
            }

        }
    }

    private void addToList(Vector3 v)
    {
        if (vList != null)
        {
            Vector3 add = v;
            add.z = 10;
            vList.Add(Camera.main.ScreenToWorldPoint(add));

        }
        else
        {
            print("trying to add to a not ongoing list. addToList()");
        }
    }

    private bool goodAngle(Vector3 v)
    {
        if (vList.Count >= 3)
        {
            Vector3 first = vList[0];
            Vector3 second = vList[1];
            Vector3 secondLast = vList[vList.Count - 2];
            Vector3 last = vList[vList.Count - 1];
            //print("first: " + first + " , second: " + second + " , last: " + last);
            Vector3 v1 = second - first;
            Vector3 v2 = last - second;
            //print("v1: " + v1 + " , v2: " + v2);
            float angle1 = Vector3.Angle(v1, v2);
            if (angle1 > 80)
            {
                print("whole: " + angle1);
                return false;
            }
            v1 = secondLast - first;
            v2 = last - secondLast;
            //print("v1: " + v1 + " , v2: " + v2);
            angle1 = Vector3.Angle(v1, v2);
            if (angle1 > 45)
            {
                print("last: " + angle1);
                return false;
            }
        }
        return true;
    }

    private bool vectorsDiffer(Vector3 first, Vector3 second)
    {
        first.z = 10;
        Vector3 fir = Camera.main.ScreenToWorldPoint(first);
        Vector3 difference = fir - second;
        float X = Mathf.Abs(difference.x);
        float Y = Mathf.Abs(difference.y);
        if (X > differThroshold || Y > differThroshold)
        {
            return true;
        }
        return false;
    }

    public List<Vector3> getList()
    {
        if (vList != null)
        {
            return vList;
        }
        if (trailList.Count != 0)
        {
            return trailList[trailList.Count - 1];
        }
        return null;
    }

    public void startTrail(Vector3 startPoint)
    {
        if (vList == null)
        {
            vList = new List<Vector3>();
            addToList(startPoint);
        }
        else
        {
            print("trying to start list when one is ongoing. startTrail()");
        }
    }

    public void endTrail(Vector3 endPoint)
    {
        if (vList != null)
        {
            addToList(endPoint);
            trailList.Add(vList);
            vList = null;
        }
        else
        {
            print("trying to end a null list. endTrail()");
        }
    }

}
