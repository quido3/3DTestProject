using UnityEngine;
using System.Collections;

public class LiquidScript : MonoBehaviour
{

    public GameObject liquid;
    public SceneScript sceneHandler;
    float full = 500;
    Material m;

    // Use this for initialization
    void Start()
    {
        full = 500 + PlayerPrefs.GetInt(SS.Level) * 250;
        m = liquid.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private float aboutWholeMeshSize = 80;

    public void addLiquid(float meshArea)
    {
        float current = m.GetFloat("_Cutoff");

        float toAdd = (meshArea / aboutWholeMeshSize) * 200;
        print(toAdd / full);
        print(current);
        current -= toAdd / full;
        print(current);
        m.SetFloat("_Cutoff", current);
        sceneHandler.addPoints((int)(toAdd));
        if (current <= 0)
        {
            sceneHandler.levelClear();
        }
    }
}
