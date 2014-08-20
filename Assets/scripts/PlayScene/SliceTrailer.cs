using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SliceTrailer : MonoBehaviour
{

    List<List<Vector3>> trailList = new List<List<Vector3>>();
    List<Vector3> vList = null;

    private float differThroshold = 0.7f;

    public void Add(Vector3 v)
    {
        if (vectorsDiffer(v, vList[vList.Count - 1]))
        {
            addToList(v);
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
