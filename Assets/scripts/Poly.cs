using UnityEngine;
using System.Collections;

public class Poly : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static bool ContainsPoint(Vector3[] polyPoints, Vector3[] ps)
    {

        foreach (Vector3 p in ps)
        {
            int j = polyPoints.Length - 1;
            bool inside = false;
            for (int i = 0; i < polyPoints.Length; j = i++)
            {
                if (((polyPoints[i].y <= p.y && p.y < polyPoints[j].y) || (polyPoints[j].y <= p.y && p.y < polyPoints[i].y)) &&
                   (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x))
                    inside = !inside;
            }
            if (inside == false)
            {
                return false;
            }
        }
        return true;
    }
}
