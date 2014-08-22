﻿using UnityEngine;
using System.Collections;

public class TextureScroller : MonoBehaviour {

	private float scrollSpeed = 0.01F;
		
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
			float offset = Time.time * scrollSpeed;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
			
		
	}
}