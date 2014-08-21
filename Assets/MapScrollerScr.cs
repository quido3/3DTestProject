using UnityEngine;
using System.Collections;

public class MapScrollerScr : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - 0.01F);
	}
}
