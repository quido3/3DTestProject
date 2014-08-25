using UnityEngine;
using System.Collections;

public class SS : MonoBehaviour
{

    public static string Level = "LevelPref";
    public static string Points = "PointsPref";

    private static LayerMask lMask = 1 << 0;

    public static GameObject MyInstantiate(GameObject prefab, Vector3 pos, GameObject parent)
    {
        bool goodSpot = false;
        Vector3 spot = Vector3.zero;
        spot.z = -1;
        while (goodSpot == false)
        {
            spot.x = Random.Range(-5, 5);
            spot.y = Random.Range(-5, 5);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(spot, Vector3.forward, out hit, Mathf.Infinity, lMask))
            {
                goodSpot = true;
            }
        }
        spot.z = 0;

        GameObject go = (GameObject)Instantiate(prefab);
        go.transform.parent = parent.transform;
        go.transform.localPosition = spot;
        go.transform.localRotation = Quaternion.identity;
        return go;
    }

    public static void MyInstantiate(GameObject prefab, Vector3 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity);
    }

    public static bool IsItInsideMesh()
    {
        return false;
    }
}
