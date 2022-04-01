using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    //IMP: Tick the "Apply Root Motion" checkbox.
    //IMP: HashID - Every Unity Component has an unique "HashID" or an "InstanceID" which Unity uses as a reference. (particularly to get Components)
public class GuardAnimator  //Part II
{
    private NavMeshAgent meshAgent;
    private Animator animator;
    private float turnAmount;
    private float forwardAmount;
    private bool UpdatedTurnThisFrame;  //Default Value: False
    private bool UpdatedMoveThisFrame;  //Default Value: False
    private bool LocomotionStateExists;
    private string LocomotionStateName = "Locomotion BlendTree";
    public bool isInLocomotion;


    public GuardAnimator(NavMeshAgent meshAgent, Animator animator)
    {
        this.meshAgent = meshAgent;
        this.animator = animator;

        //HasState - Retuns has State: True or False. (same as Boolean)
        //LayerIndex - The Index in the Animator > Layer. In our case it is "0" as we only have only one layer "BaseLayer".
        //StateID - Every State has an ID. (created by Untiy)
        //Animator.StringToHash - Converts a "String" to "Hash". We use this bcoz we use the name of Blend Tree "Locomotion BlendTree".
        //So we need to convert it into a Hash value, which can be used as a "StateID".
        LocomotionStateExists = animator.HasState(0, Animator.StringToHash(LocomotionStateName));

        if(!LocomotionStateExists)
        {
            //$ - An alternate to "True for + {animator.gameObject.name} + because we can't find a state named + {LocomotionStateName}");
            //Instead of using "+" multiple times, we can simply use "$" in the begining and Angular Brackets "{ }" to define a component.
            Debug.LogWarning($"isInLocomotion will always be True for {animator.gameObject.name} because we can't find a state named {LocomotionStateName}");
        }
    }

    // Update is called once per frame
    public void Update()
    {
        DecayMovementateToZero();
        DecayTurnRateToZero();

        if(!meshAgent.hasPath)
        {
            return;
        }
        else
        {
            //desiredVelocity - Tells us where the NavMeshAgent(Guard) wants to move immediately.
            Move(meshAgent.desiredVelocity);

            if(LocomotionStateExists)
            {
                isInLocomotion = animator.GetCurrentAnimatorStateInfo(0).IsName(LocomotionStateName);
            }
        }
    }

    private void Move(Vector3 move)
    {
        if(move.magnitude > 1f)
        {
            //Bcoz we set up in Blend Tree values between 0 to 1 and if the Guard is running
            //at a speed greater than 1 (suppose 4), then we would want to bring that speed
            //amount back to 1.
            move.Normalize();   //Always return value "1".
        }

        //TransformDirection - Transforms a direction from 'local space' to 'world space'. 
        //InverseTransformDirection - Transforms a direction from 'world space' to 'local space'.
        //We need to use this function as we want our Guard to move around the Local Axis. 
        move = meshAgent.transform.InverseTransformDirection(move);

        //To keep the Guard enatcted to the Plane(Floor GameObject in the scene).
        move = Vector3.ProjectOnPlane(move, Vector3.up);

        //Returns radian value between X & Z.
        //Rotates the Guard.
        turnAmount = Mathf.Atan2(move.x, move.z);

        //Simply meshAgent's Forward Direction.
        forwardAmount = move.z;

        turnAmount = Mathf.Clamp(turnAmount, -1.0f, 1.0f);  //We use -1 & 1 as our Guard can rotate Left or Right i.e., in Opposite Directions.
        forwardAmount = Mathf.Clamp01(forwardAmount); //bcoz Guard's Animator Blend Tree has values between 0  to 1.

        //Damping - Slwoly moving the values from Initila value to Final value. Same as Linear Interpolation.
        animator.SetFloat("TurnRate", turnAmount, 0.15f, Time.deltaTime);
        animator.SetFloat("MoveSpeed", forwardAmount, 0.35f, Time.deltaTime); 

        UpdatedMoveThisFrame = true;
    }

    public void TurnOnSpot(float turnRate)  //From PatrolRotate > TurnOnSpot(rotateScaleSpeed * rotateSpeed)
    {
        //Slowly lerp from 0.15f towards turnRate.
        animator.SetFloat("TurnRate", turnRate, 0.15f, Time.deltaTime);

        //Slowly lerp from 0.15f towards 0.0f.
        animator.SetFloat("MoveSpeed", 0.0f, 0.5f, Time.deltaTime);       

        //We set it "Ture" bcoz we want to Rotate the Guard only on this frame, and not other. 
        //Then after this function it goes to "DecayTurnRateToZero()" function, which sets it value again to "False" and so the animation is performed only once.
        UpdatedTurnThisFrame = true;    
    }
    
    private void DecayTurnRateToZero()
    {
        //If we have set Turn Rate this frame, switch the flag to False so we don't adjust Turn Rate
        //Simply means: If(UpdatedTurnThisFrame == true)
        if(UpdatedTurnThisFrame)    //Default Value: false
        {
            UpdatedTurnThisFrame = false;
            return;
        }

        animator?.SetFloat("TurnRate", 0.0f, 0.15f, Time.deltaTime);
    }

    private void DecayMovementateToZero()
    {
        //If our Pathing's set to a Move Speed  this frame, switch the flag to False so we don't adjust it
        //Simply means: If(UpdatedMoveThisFrame == true)
        if(UpdatedMoveThisFrame)    //Default Value: false (Changes after the First Frame Update: True)
        {
            UpdatedMoveThisFrame = false;
            return;
        }

        animator?.SetFloat("MoveSpeed", 0.0f, 0.15f, Time.deltaTime);
    }
 
}
