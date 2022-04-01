using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController
{
    private GameObject cameraObject;
    private GameObject mainCameraTransform;
    private Transform cameraPosition;
    private Vector3 cameraOffset;
    private Transform player;
    private Transform eyes;

    //private PlayerInputCallbacks inputCallbacks;
    private ViewOcclusionManager viewOcclusionManager;  //S2 - Assignment 01 - Part II

    //private float sensitivityX = 0.05f;
    //private float sensitivityY = 0.05f;

    public GameObject pauseMenu;
    public CameraController(GameObject cameraObjectPrefab, Transform parent, Transform player)
    {
        this.player = player;
        //this.inputCallbacks = inputCallbacks;
        cameraObject = GameObject.Instantiate(cameraObjectPrefab, null);
        cameraObject.name = "Camera";

        eyes = player.Find("Eyes");

        /*inputCallbacks.OnPlayerLookFired += (lookDelta) =>
        {
            UpdateLookDirection(lookDelta);
        };*/

        mainCameraTransform = cameraObject.transform.Find("MainCamera").gameObject;

        cameraOffset = mainCameraTransform.transform.position - player.position;    //S2 - Assignment 01

        viewOcclusionManager = new ViewOcclusionManager(this);  //S2 - Assignment 01 - Part II
    }

    private void UpdateCameraFollow()
    {
        //mainCameraTransform.transform.position = eyes.position;
        
        mainCameraTransform.transform.position = player.position + cameraOffset;    //S2 - Assignment 01
    }

    /*private void UpdateLookDirection(Vector2 lookDelta)
    {
        mainCameraTransform.transform.Rotate(Vector3.up, lookDelta.x * sensitivityX, Space.World);
        mainCameraTransform.transform.Rotate(Vector3.left, lookDelta.y * sensitivityY, Space.Self);
    }*/

    public void Update()
    {
        viewOcclusionManager.Update();  //S2 - Assignment 01 - Part II
        UpdateCameraFollow();
    }

    public Transform Target
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
            player = value;
        }
    }

    public Transform CameraTransform
    {
        get
        {
            return cameraObject.transform;
        }
    }

    public Transform MainCameraTransform
    {
        get
        {
            return mainCameraTransform.transform;
        }
    }
}
