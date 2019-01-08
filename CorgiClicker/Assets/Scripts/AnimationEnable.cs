using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnable : MonoBehaviour {

	public string myParam;
	public Animator myAnimator;

	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator>();
	}
	
	public void SetParamTrue()
	{
		//myAnimator.Play(animationName);
		myAnimator.SetBool(myParam, true);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
