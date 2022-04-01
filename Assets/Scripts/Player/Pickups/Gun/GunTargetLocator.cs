using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTargetLocator   //S2 - Assignment 02
{
    public GuardManager guardManager; 
    public PlayerObjectData playerObjectData; 
    private PlayerSettings playerSettings;  //S2 - Assignment 04
    //private float shotCheckRadius = 15f;
    //private float shotAngleRadius = 60f;

    public GunTargetLocator(GuardManager guardManager, PlayerObjectData playerObjectData, PlayerSettings playerSettings)   //S2 - Assignment 04
    {
        this.guardManager = guardManager;
        this.playerObjectData = playerObjectData;
        this.playerSettings = playerSettings;
    }

    public void Locate(Action<Transform> OnSuccess)
    {
        //It locates the Nearest Enemy among all the other Enemies.
        Transform closestEnemy = null;

        //We run a Check for all/each the Guard(s) present inside the Player's Aim Vision.
        //Note: While using KeyValuePair, you should use Dictionary variables and not any other than that.
        foreach(KeyValuePair<Guards, GuardController> guard in guardManager.guardDict)
        {
             if(guard.Value.CanBeTargeted == false)
            {
                continue;
            }

            //Position of the Guard.
            Vector3 guardPosition = new Vector3(guard.Key.transform.position.x, guard.Key.visionData.eyeHeight, guard.Key.transform.position.z);

            //Calculate the Distance from the Guard's Head or Eye Position to the Player's Head Position.
            Vector3 playerToEnemy = guardPosition - playerObjectData.Head.position;

            //Ray from Player's Head towards Guard's Head.
            Ray rayToEnemy = new Ray(playerObjectData.Head.position, playerToEnemy);
            Debug.DrawRay(playerObjectData.Head.position, playerToEnemy, Color.yellow);

            //Hit: Stores & Returns the info. of the Objects that it has Hit.
            RaycastHit hit;
            if(Physics.Raycast(rayToEnemy, out hit, playerSettings.shotCheckRadius))
            {
                Debug.Log(hit.collider.name);
                //We check if the Hit has Hit a Target with Tag "Enemy", then go into the below Function.
                if(hit.transform.tag == "Enemy")
                {
                    Vector3 distance = guard.Key.transform.position - playerObjectData.Head.position;
                    float angle = Vector3.Angle(distance, playerObjectData.Head.forward);
                    Debug.DrawRay(distance, playerObjectData.Head.position);

                    //Absolute (Abs) = Returns Positive Integer. Even if the Integer Value is -ve, its Absolute Value will be in +ve
                    //Like if Angle is "-75", its Abs will be "75".
                    if(Mathf.Abs(angle) <= playerSettings.shotAngleRadius)
                    {
                        //This is the First Enemy that our Player Sees or Detects.
                        //And then it assigns that Guard's transfrom to the "closestEmeny" transform varialbe.
                        //And from the 2nd Guard on, the code will directly go into the "esle" part & execute its logic.
                        if(closestEnemy == null)
                        {
                            closestEnemy = guard.Key.transform;
                        }

                        //Here we check over each Guard in the Player's Vision Range (shotAngleRadius) and calculate its ClosestDistance.
                        //We Iterate this Process for Each Guard.
                        //Then we compare Each Guard's Distance, and see if it is Smaller than the Previous Guard.
                        //If it is, then we assign that Guard as "closestEnemy" and pass in that value to other scripts.
                        else
                        {
                            //We make the Positions of the Player & the Guard FLAT.
                            //It helps in calculating the Distance. As only the X & Z components are to be calulated. The Y component is Zero.
                            Vector3 playerPositionFloor = playerObjectData.Head.position;
                            playerPositionFloor.y = 0;

                            Vector3 guardPositionFloor = guard.Key.transform.position;
                            guardPositionFloor.y = 0;

                            //We calculate the Distance of each Guard found after the First Guard. (i.e., Guard2, Guard3, so on...)
                            float checkingAllGuardDistance = Vector3.Distance(playerPositionFloor, guardPositionFloor);
                       

                            //Then we compare whether the Distance of Each Next or New Gaurd found, is Smaller than the Distance of the First Guard. (i.e., Guard1.)
                            //That is, the one from the "if(closestEnemy == null)" condition.
                            //And if it satisfies as True, then we stores that Guard, (Guard2, Guard3, so on...Whichever has Smallest Distance from Player) as Closest Enemy.
                            //if not, then it considers the above guard from Line 61, the First Guard as the ClosestGuard.
                            //This Process Iterates every time for each Guard, as it is inside a "Foreach-Loop".
                            //And once we find the Guard that has the Smallest Distance from the Player, we pass that Guard as the "Closest Guard" and run a check along with the 
                            //Previous Closest Guards. Then we iterate with each Guard and try to find the Guard that has the Smallest Distance from Player.
                            //And once we find that, we then pass it along other scripts on Line 106. 
                            Vector3 closestGuardPositionFloor = closestEnemy.position;
                            closestGuardPositionFloor.y = 0;
                            float currentClosestGuardDistance = Vector3.Distance(playerPositionFloor, closestGuardPositionFloor);

                            //This checks whether Current Guard's Distance is Smaller than the Previous Guard's Distance.
                            if(checkingAllGuardDistance < currentClosestGuardDistance)
                            {
                                closestEnemy = guard.Key.transform;
                            }
                            
                        }
                    }
                }
            }
        }

        OnSuccess?.Invoke(closestEnemy);
        //We could have used a Return Function type, where we would return this Transform value,
        //But it does not provide More Functionalities.

    }
}
