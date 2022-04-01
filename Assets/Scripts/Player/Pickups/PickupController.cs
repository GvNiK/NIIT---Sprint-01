using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController 
{
    private Transform levelObjects; //levelObjects = "LevelController" GameObject's Transform in each Level.
    private PlayerController playerController;
    private List<Pickup> requiredPickups;
    private List<Pickup> activePickups;
    private PickupEvents events;

    public PickupController(Transform levelObjects, PlayerController playerController, PickupEvents pickupEvents)
    {
        this.levelObjects = levelObjects;
        this.playerController = playerController;
        this.events = pickupEvents;

        FindPickupsInLevel(levelObjects);
    }

    private void FindPickupsInLevel(Transform levelObjects)
    {
        requiredPickups = new List<Pickup>();
        activePickups = new List<Pickup>();

        Transform pickupRoot = levelObjects.Find("Pickups");    //Pickups = is a Empty GameObject under LevelController.

        if(pickupRoot == null)
        {   
            Debug.LogError("PickupController tried to find 'Pickups' GameObject, but failed!");
            return;
        }
        
        //pickupRoot = Pickup GameObjects container. (Contains all the pickupTrans)
        //pickupTrans = Pickups GameObjetcs child under "pickupRoot"
        foreach (Transform pickupTrans in pickupRoot)
        {
            Pickup pickup = pickupTrans.GetComponent<Pickup>();

            if(pickup != null)
            {
                SetupPickup(pickup);
                events.OnPickupEventCollected(pickup);
            }
        }
    }

    private void SetupPickup(Pickup pickup)
    {
        activePickups.Add(pickup);  //Bcoz it is our Current Pickup Object.

        CollisionCallbacks collisionCallbacks = pickup.GetComponentInChildren<CollisionCallbacks>();
        Action<Collider> OnPickupCollision = null;  //We create this separately, as we need to Subscribe & UnSubscribe from the Pickup Events.

        //Workflow: OnTriggerEntered > OnPickupCollision > Lambda Function.
        //Bcoz Unity looks where "OnPickupCollision" is called or subscribed and then executes it and then enters the Lambda Expression.
        //No matter where you place the "OnPickupCollision" Subscriber before the Lamba Expression or after, it would look around to the
        //Subscriber and execute its commands respectively

        OnPickupCollision = (collider) =>
        {
            //The below code executes such as if we Pickup the Item once, we wont be able to Pick it up again.
            //Despite if we do not Set the object Active to false or Unhide it from the Inspector.
            
            if(playerController.OwnsCollider(collider))     //Checks for Player's seperate Own Collsions
            {
                collisionCallbacks.OnTriggerEntered -= OnPickupCollision;   //Garbage Code clean up.

                //We remove our Current & Required Pickups in Order to perform the Drop Command
                activePickups.Remove(pickup);   
                if(requiredPickups.Contains(pickup))
                {
                    requiredPickups.Remove(pickup);
                }

                events.OnPickupEventCollected(pickup);
            }
        };

        collisionCallbacks.OnTriggerEntered += OnPickupCollision;

        if(pickup.requiredForLevelCompletion)
        {
            requiredPickups.Add(pickup);    //Adds Current Pickups under Required for LevelCompletion context.
        }
    }

}
