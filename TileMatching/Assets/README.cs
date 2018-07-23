using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class README : MonoBehaviour {

	// Use this for initialization
	public void Start () {
		Debug.LogWarning("Hello, there are two implementations of this game");
		Debug.LogWarning("SampleScene is just straight solution without any flexibility");
		Debug.LogWarning("SampleScene2 is flexible solution with reading textures from /Resources folder");
		Debug.LogWarning("Generating field with desired parameters and so on");
		Debug.LogWarning("Both samples are useful to understand basics of unity 3d");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
