using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureScroller : MonoBehaviour
{

    private float topScrollSpeed = 0.03F;
    private float scrollSpeed = 0;

    int level;

    public List<float> levelOffsets = new List<float>();

    Material mat;

    bool travel = false;

    public ScrollHandler scroller;

    bool travelled = false;
    public GameObject fuelBtn;

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
            if (scrollSpeed < topScrollSpeed)
            {
                scrollSpeed += topScrollSpeed / 250;
            }
            scroller.startScrolls();
            if (mat.GetTextureOffset("_MainTex").y < levelOffsets[level - 1])
            {
                float offset = mat.GetTextureOffset("_MainTex").y + Time.deltaTime * scrollSpeed;
                renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
            }
            else
            {
                travel = false;
                travelled = true;
                scroller.stopScrolls();
                fuelBtn.GetComponent<SpriteRenderer>().enabled = true;
                fuelBtn.GetComponent<CircleCollider2D>().enabled = true;
                foreach (Transform go in fuelBtn.transform)
                {
                    go.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
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
