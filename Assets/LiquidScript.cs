using UnityEngine;
using System.Collections;

public class LiquidScript : MonoBehaviour
{

    public GameObject liquid;
    float full = 500;
    Material m;

    // Use this for initialization
    void Start()
    {
        m = liquid.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addLiquid(float meshArea)
    {
        float current = m.GetFloat("_Cutoff");

        float toAdd = meshArea / full;

        current -= toAdd;
        m.SetFloat("_Cutoff", current);

    }
}
