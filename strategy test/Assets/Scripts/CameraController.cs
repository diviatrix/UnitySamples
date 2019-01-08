using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float zoomSpeed; // camera zoom speed
    public float camSpeed; // camera move speed
    public float rotSpeed; // camera move speed
    public float zLimit; // vertical camera limit

    

    // private stuff
    private bool isOrtho; // is camera orthographic
    Camera cameraComponent; // just link for camera
    private GameObject playerObject; // link for player parent object
    private Transform playerTransform; // link for player parent transform
    


    // GAME CONTROLLER ->>>>
    private GameController gameControllerObject;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        gameControllerObject = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        cameraComponent = GetComponentInChildren<Camera>(); // search for camera component

        // set ortho camera mode
        isOrtho = cameraComponent.orthographic;

        // set player object and transform
        playerObject = GameObject.Find("Player");
        playerTransform = playerObject.transform; 
    }

    // Update is called once per frame
    void Update()
    {
        // camera ortho switch
        // dunno why i need this
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerTransform.Rotate(Vector3.up, 90);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (isOrtho)
            {
                cameraComponent.orthographic = false;
                isOrtho = false;
            }
            else
            {
                cameraComponent.orthographic = true;
                isOrtho = true;
            }
        }        

        // camera rot
        if (Input.GetMouseButton(1))
        {
            CameraRotationHandler();
        }        

        // camera dnd
        if (Input.GetMouseButton(2))
        {
            CameraMovementHandler();
        }

        CameraZoomHandler();
    }

    void CameraRotationHandler()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            cameraComponent.transform.Rotate(Input.GetAxis("Mouse Y") * -rotSpeed, 0, 0, Space.Self);
        }
        else
        {
            playerTransform.Rotate(0, Input.GetAxis("Mouse X") * rotSpeed, 0, Space.Self);
        }
    }

    void CameraMovementHandler()
    {
        playerTransform.position -= playerTransform.right * Input.GetAxis("Mouse X") * camSpeed;
        playerTransform.position -= playerTransform.forward * Input.GetAxis("Mouse Y") * camSpeed;
    }

    void CameraZoomHandler()
    {
        // camera ortho zoom
        if (isOrtho)
        {
            if (cameraComponent.orthographicSize >= zLimit)
            {
                cameraComponent.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            }

            // push to zlimit if camera is too close
            if (cameraComponent.orthographicSize < zLimit)
            {
                cameraComponent.orthographicSize = zLimit;
            }
        }
        // camera perspective zoom
        if (!isOrtho)
        {
            if (playerTransform.position.y >= zLimit)
            {
                playerTransform.position -= playerTransform.up * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            }

            // push to zlimit if camera is too close
            if (playerTransform.position.y < zLimit)
            {
                playerTransform.position = new Vector3(playerTransform.position.x, zLimit, playerTransform.position.z);
            }
        }
    }
}