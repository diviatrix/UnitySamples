using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteraction : MonoBehaviour {

    // delay for blocks settings
    float lastClickTime;
    public float clickDelay;
    bool readyToClick = true;

    // initialize method
    void Start ()
    {
        lastClickTime = Time.time;
    }

	// Update is called once per frame
	void Update () {
        HandleMouseClicks();        
    }



    // Mouse clicks login
    void HandleMouseClicks()
    {
        // check  if delay passed
        if (lastClickTime + clickDelay > Time.time)
        {
            readyToClick = false;
        }
        else readyToClick = true;

        // check if player clicked on any collider
        // and destroy block with LMB
		if ((Input.GetAxis("Fire1") > 0) && (readyToClick)) 
		{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10))
            {
                Destroy(hit.transform.gameObject);
            }
            lastClickTime = Time.time;
        }
        // and create block with RMB
        if ((Input.GetAxis("Fire2") > 0) && (readyToClick))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10))
            {                
                GameObject newBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);

                
                if (!IsBlockOver(hit))
                {
                    newBlock.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
                }                
            }

            lastClickTime = Time.time;
        }
    }


    // check is there is block over one we want to build
    // this is to prevent multiple building in same block

    bool IsBlockOver(RaycastHit go)
    {
        Ray blockRay = new Ray(go.transform.position, go.transform.up);
        RaycastHit hit;

        if (Physics.Raycast(blockRay, out hit, 1)) return true;
        else return false;              
    }    
}
