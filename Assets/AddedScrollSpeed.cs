using UnityEngine;
using System.Collections;

public class AddedScrollSpeed : MonoBehaviour {
	
	public float speed;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		ParallaxController.addedScroll = speed;
		
	}
}
