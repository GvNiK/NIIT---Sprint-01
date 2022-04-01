using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour
{
    private PlayerInputCallbacks inputCallbacks;
    public GraphicRaycaster graphicRaycaster;
    public GameObject joystick; //We want to Move the Joystick.
    public Image up;
    public Image down;

    public Image left;

    public Image right;


    public void Setup(PlayerInputCallbacks inputCallbacks)
    {
        this.inputCallbacks = inputCallbacks;

        inputCallbacks.OnPlayerTapFired += () => OnTap();
        inputCallbacks.OnPlayerTapReleased += () => OnTapReleased();

        inputCallbacks.OnPlayerMoveFired += (move) => UpdateMoveHeat(move);
    }

    private void UpdateMoveHeat(Vector2 movement)
    {
        Color full = Color.white;   //Full Alpha

        //UP
        full.a = movement.y;    //a: Alpha component of the color.
        up.color = full;

        //DOWN
        full.a = -movement.y;
        down.color = full;

        //RIGHT
        full.a = movement.x;
        right.color = full;

        //LEFT
        full.a = -movement.x;
        left.color = full;

    }

    private void OnTap()
    {
        //Mouse Pointer Setup
        Vector2 mousePosition = Pointer.current.position.ReadValue(); 
        PointerEventData ped = new PointerEventData(null);
        ped.position = mousePosition;   //Assign MousePosition to the PED.

        List<RaycastResult> results = new List<RaycastResult>();    //This holds a List of RayCasts that occured, when we Touch the Screen - Space.

        //The Graphic Raycaster is used to raycast against a Canvas. 
        //The Raycaster looks at all Graphics on the canvas and determines if any of them have been hit.
        graphicRaycaster.Raycast(ped, results);
        //Debug.Log("OnTap");

        foreach(RaycastResult result in results)
        {
            if(result.gameObject.tag.Equals("ValidJoystickArea"))   //Glitch: result = intsead of graphicsRaycaster.
            {
                ActivateJoystick(result.screenPosition);    //Position on the Screen where we Tap.
            }
        }
    }

    private void ActivateJoystick(Vector2 hitScreenPosition)    //Move the Joystick anywhere along JoyStickArea.
    {
        joystick.transform.position = hitScreenPosition;
        joystick.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    private void OnTapReleased()
    {
        inputCallbacks.OnPlayerMoveFired(Vector2.zero);   //Safety - MoveFired holds the Player Movement Control.
        joystick.GetComponent<CanvasGroup>().alpha = 0.3f;
    }
}
