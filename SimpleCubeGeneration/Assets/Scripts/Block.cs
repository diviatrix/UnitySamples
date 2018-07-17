using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public bool randomizeColor = false;
	public bool beingLooked = false;

	public List<Color> Colors = new List<Color> {Color.grey, Color.white};

	Material blockMaterial;
	// Use this for initialization
	void Start () 
	{
		blockMaterial = transform.GetComponent<Renderer>().material;

		// randomize color of block if setup in editor
		if (randomizeColor) blockMaterial.color = new Color(Random.value,Random.value,Random.value,Random.value);
		else blockMaterial.color = Colors[0];		
	}
	
	// Update is called once per frame
	void Update () {
		if (beingLooked) blockMaterial.color = Colors[1];
		else blockMaterial.color = Colors[0];
	}
}
