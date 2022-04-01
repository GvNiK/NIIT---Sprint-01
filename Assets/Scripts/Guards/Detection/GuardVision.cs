using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GuardVision 
{
    public Action<List<GameObject>> OnObjectsInView = delegate { };
    public Action OnNoObjectsInView = delegate { };
    public List<GameObject> objectsInCone;
    public VisionData visionData;
    private Player player;
    private Guards guard;
    public Transform playerHead;
    private bool visionEnabled = true;

    public GuardVision(Guards guard, VisionData visionData, Player player)
    {
        this.guard = guard;
        this.visionData = visionData;
        this.player = player;
        objectsInCone = new List<GameObject>();
        playerHead = player.ObjectData.Head.transform;
    }

    // Update is called once per frame
    public void Update()
    {
        objectsInCone.Clear(); //IMP: To delete Garbage Data.

        if(visionEnabled == false)
        {
            return;
        }

        //This cretaes an Empty Transform GameObject onto the Gurad.
        //"guard.transform.position.y + visionData.eyeHeight" - This line increments the "Y" position value,
        //so as to bring it up to the eye level of the Guard.
        Vector3 eyePosition = new Vector3   
        (
            guard.transform.position.x, 
            guard.transform.position.y + visionData.eyeHeight, 
            guard.transform.position.z
        );

        //Calculating Distance and Angle between Player from the EyePosition of Enemy.
        Vector3 distance = playerHead.position - eyePosition;  //Did no use magnitude here bcoz we need to pass a Vector3 value in the "angle" field below.  
        float angle = Vector3.Angle(distance, guard.transform.forward); 

        //Debug.DrawRay(eyePosition, guard.transform.forward * 5, Color.green);

        //Used magnitude here bcoz the radius and angle values are in "float".
        if(distance.magnitude < visionData.radius && angle < visionData.angle || distance.magnitude <= visionData.awarnessZone)  
        {
            float raycastDistance = Vector3.Distance(eyePosition, playerHead.position);   //Added here for Performance sake.
            if(!Physics.Raycast(eyePosition, distance, visionData.radius, visionData.raycastMask))
            {
                objectsInCone.Add(player.ObjectData.gameObject);
                //Debug.Log(objectsInCone);
            }
        }

        //Checks whether the Player is in the View Radius of the Guard or Not.
        if(objectsInCone.Count == 0)    
        {
            OnNoObjectsInView();
        }
        else
        {
            OnObjectsInView(objectsInCone);
        }
        
        if(visionData.visualize)    //Default value = false 
        {
            DrawAwarenessZone();
            DrawCone();
        }
        
    }

    public void Enable() 
    {
        visionEnabled = true;    
    }
    public void Disable() 
    {
        visionEnabled = false;    
    }
    private void DrawCone()
    {
        /////* Draw Outer Lines *//////

        //Draws a line starting from the Guard's Foward Direction(Origin Point) 
        //to the Radius of the Circle(visionData.radius).
        Vector3 scaledForward = guard.transform.forward * visionData.radius;

        //Rotates the line drawn.(in +ve)
        Vector3 rotatedForward = Quaternion.Euler(0, visionData.angle, 0) * scaledForward;
        Debug.DrawRay(guard.transform.position, rotatedForward, Color.green);

        //Rotates the line drawn in Opposite Direction.(i.e., in -ve)
        rotatedForward = Quaternion.Euler(0, -visionData.angle, 0) * scaledForward;
        Debug.DrawRay(guard.transform.position, rotatedForward, Color.green);
        

        //////* Draw Inner Lines *///////

        var rayColor = objectsInCone.Count <= 0 ? Color.white : Color.red;  //Ternary Operator
        
        //Creates line(s). The value "5" represents Degree Angles i.e., for each 5 Degrees, 
        //create 'n' number of lines.(visionData.raycastLines)
        int iterations = ((int)visionData.angle / 5) * visionData.raycastLines;     
        
        for(int i = 1; i < iterations; i++)
        {
            //The value 2 is used as we are using Two Lines to craete the Cone.
            float rotateAmount = visionData.angle / iterations * 2 * i - visionData.angle;  
            rotatedForward = Quaternion.Euler(0, rotateAmount, 0) * scaledForward;
            Debug.DrawRay(guard.transform.position, rotatedForward, rayColor);
        } 
    }

    //If Player tries to seek from behind the Guard, then Guard detects the Player.
    private void DrawAwarenessZone()
    {
          var rayColor = objectsInCone.Count <= 0 ? Color.blue : Color.yellow;  //Ternary Operator

          for(int i = 0; i < 36; i++)
          {
              Vector3 endPoint = Quaternion.Euler(0, i * 10, 0) * new Vector3(0, 0, visionData.awarnessZone);
              Debug.DrawLine(guard.transform.position, guard.transform.position + endPoint, rayColor); 
          }
    }
   
}
