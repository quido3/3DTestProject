using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour {

	public float scrollSpeed;
	public static float addedScroll;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
		Vector3 pos = this.transform.position;
		
		pos.x -= scrollSpeed + addedScroll;
		
		this.transform.position = pos;
		
		print (gameObject.transform.localPosition.x + "Tämän edellä näkyvän pitäisi olla pienempi kuin tämän: " + this.transform.collider2D.bounds.size.x * -1);
		
		//print (this.gameObject.tag + this.transform.localPosition);
		
		if (gameObject.transform.localPosition.x < this.transform.collider2D.bounds.size.x * -1) {
			gameObject.transform.localPosition = new Vector3 (this.transform.collider2D.bounds.size.x, this.transform.localPosition.y, 0 );
			print ("Nyt mun taytyy liikkuu");
			
		}
		
		
	}
}
