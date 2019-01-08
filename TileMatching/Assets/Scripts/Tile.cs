using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Tile : MonoBehaviour {

	public bool isDeath = false;
	public Sprite back,front;	

	// bool which changes texture of current sprite according to status
	// set private one to store it inside
	private bool _isOpen;

	public bool isOpen
	{
		get { return _isOpen; }

		set
		{
			_isOpen = value;
			
			if (_isOpen == true)
			{
				//Debug.Log("Set true");

				GetComponent<SpriteRenderer>().sprite = front;
			}
			else 
			{
				//Debug.Log("Set False");
				GetComponent<SpriteRenderer>().sprite = back;
			}
		}
	}

	// switch status of tile
	public void SwitchOpen()
	{
		isOpen = !isOpen;
	}

	void Start()
	{

	}
	
}
