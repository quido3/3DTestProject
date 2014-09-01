using UnityEngine;
using System.Collections;

public class LiquidScript : MonoBehaviour
{

    public GameObject liquid;
    public SceneScript sceneHandler;
    float full = 500;
    float currLiquid = 0;
    public TextMesh litersTXT;
    public TextMesh kmsTXT;
    Material m;

    // Use this for initialization
    void Start()
    {
        int lvl = PlayerPrefs.GetInt(SS.Level);
        if (lvl >= 7)
        {
            full = 10000;
        }
        else
        {
            full = 500 + lvl * 250;
        }
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
        currLiquid += toAdd;
        current -= toAdd / full;
        litersTXT.text = ((int)currLiquid).ToString();
        kmsTXT.text = ((int)(currLiquid * 2.2f)).ToString();
        m.SetFloat("_Cutoff", current);
        sceneHandler.addPoints((int)(toAdd));
        if (current <= 0)
        {
            sceneHandler.levelClear();
        }
    }
}
