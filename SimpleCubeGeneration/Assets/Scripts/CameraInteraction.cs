using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteraction : MonoBehaviour {

    // delay for blocks settings
    float lastClickTime;
    public float clickDelay;
    bool readyToClick = true;

    void Start ()
    {
        lastClickTime = Time.time;
    }

	// Update is called once per frame
	void Update () {
        // check  if delay passed
        if (lastClickTime + clickDelay > Time.time)
        {
            readyToClick = false;
        }
        else readyToClick = true;

        // check if player clicked on any collider
        // and destroy block
		if ((Input.GetAxis("Fire1") > 0) && (readyToClick)) 
		{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Destroy(hit.transform.gameObject);
            }
            lastClickTime = Time.time;
        }
        // and create block
        if ((Input.GetAxis("Fire2") > 0) && (readyToClick))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                int y = 0;

                for (int i = 0; i <= 3; i++)
                {
                    if (CheckBlockOver(hit))
                    {
                        y++;
                    }                    
                }
                GameObject newBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                
                newBlock.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + y, hit.transform.position.z);
            }
            lastClickTime = Time.time;
        }
    }

    // check is there is block over one we want to build
    // this is to prevent multiple building in same block

    bool CheckBlockOver(RaycastHit hit)
    {
        bool isBlockOver = false;

        Ray blockRay = new Ray(hit.transform.position, hit.transform.up);
        if (Physics.Raycast(blockRay, out hit))
        {
            if (hit.collider != null)
                isBlockOver = true;
        }

        return isBlockOver;
    }
}
