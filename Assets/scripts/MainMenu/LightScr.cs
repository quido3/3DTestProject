using UnityEngine;
using System.Collections;

public class LightScr : MonoBehaviour {
	
	private float lightY;
	// Use this for initialization
	void Start () {
		lightY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.position = new Vector3 (transform.position.x, lightY, transform.position.z);
	
	}
}
