using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordBehavior : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<BoxCollider>();
		//speed = speed/100 * (GetComponent<TextMesh>().text.Length);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate(new Vector3(speed/10,0,0));
		if(transform.position.x >= 9){
			Destroy(gameObject);
		}
	}
}
