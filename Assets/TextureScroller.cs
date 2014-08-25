using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureScroller : MonoBehaviour
{

    private float scrollSpeed = 0.01F;

    int level;

    public List<float> levelOffsets = new List<float>();

    Material mat;

    bool travel = false;

    public ScrollHandler scroller;

    bool travelled = false;

    // Use this for initialization
    void Start()
    {
        level = PlayerPrefs.GetInt(SS.Level);
        mat = this.GetComponent<MeshRenderer>().material;
        if (level - 2 >= 0)
        {
            mat.SetTextureOffset("_MainTex", new Vector2(0, levelOffsets[level - 2]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (travel)
        {
            scroller.startScrolls();
            if (mat.GetTextureOffset("_MainTex").y < levelOffsets[level - 1])
            {
                float offset = Time.time * scrollSpeed;
                renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
            }
            else
            {
                travel = false;
                travelled = true;
                scroller.stopScrolls();
            }
        }

    }

    public void startTravel()
    {
        travel = true;
    }

    public bool travelDone()
    {
        return travelled;
    }
}
