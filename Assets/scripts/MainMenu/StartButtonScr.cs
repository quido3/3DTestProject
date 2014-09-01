using UnityEngine;
using System.Collections;

public class StartButtonScr : MonoBehaviour {
	
	private float zRotation;
	public float rotSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		zRotation -= rotSpeed;
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, zRotation);
		
	}
}
