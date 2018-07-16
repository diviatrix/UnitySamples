using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CameraController : MonoBehaviour {
	
	// Speed of player
	// set public to make access from editor object window
	public float moveSpeed;
	
	// Vector where  player will move during update cycle
	Vector3 MoveVector = new Vector3();

	void Update ()
	{
		// player movement

        if (Input.GetAxis("Vertical") != 0)
        {
            Debug.Log(Input.GetAxis("Vertical"));
            GetComponent<Rigidbody>().AddForce(transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            Debug.Log(Input.GetAxis("Horizontal"));
            GetComponent<Rigidbody>().AddForce(transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed);
        }
	}	
}
