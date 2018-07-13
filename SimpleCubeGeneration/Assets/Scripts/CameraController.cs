using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CameraController : MonoBehaviour {
	
	// Speed of player
	// set public to make access from editor object window
	public float moveSpeed;
	
	// Vector where  player will move during update cycle
	Vector3 MoveVector = new Vector3();

	void Start ()
	{
		// lock and invisible for cursor, esc to show
		Cursor.lockState = CursorLockMode.Locked;
	}
	void Update ()
	{
		// player movement
		// assigning vector values from input axis and multiply it on speed
		// multiply on Time.deltaTime to make it independent from framerate
		MoveVector = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"))*moveSpeed*Time.deltaTime;

		// constantly translate object
		// if no inputs, vector will be 0 0 0
		transform.Translate (MoveVector);
	}	
}
