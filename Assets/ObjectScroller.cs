﻿using UnityEngine;
using System.Collections;

public class ObjectScroller : MonoBehaviour {
	
	private Vector3 startPos;
	public float speed;
	// Use this for initialization
	void Start () {
		
		startPos = transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 TreePos = transform.position;
		float xSpeed = speed * 0.6f;
		float ySpeed = speed * 0.4f;
		transform.position = new Vector3(TreePos.x - xSpeed, TreePos.y - ySpeed, TreePos.z);
		
		if (gameObject.transform.position.y < 1.2f) {
		
			this.transform.position = startPos;
			
		}
	}
}