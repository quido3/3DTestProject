using UnityEngine;
using System.Collections;

public class SS : MonoBehaviour
{
    public static void MyInstantiate(GameObject prefab, Vector3 pos, GameObject parent)
    {
        GameObject go = (GameObject)Instantiate(prefab);
        go.transform.parent = parent.transform;
        go.transform.localPosition = new Vector3(0, 0, 0);
        go.transform.localRotation = Quaternion.identity;
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
