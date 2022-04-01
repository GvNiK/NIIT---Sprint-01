using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardPursueBehavior : GuardBehavior    //Assignment - 03
{
    private Guards guards;
    private NavMeshAgent meshAgent;
    private Animator guardAnimator;
    private Transform targetObject;
    private VisionData visionData;
    private GuardController guardController;
    private float pursueMoveSpeed;
    private float attackRotateSpeed;
    private bool playerInRange;
    private Vector3 targetPreviousPosition;
    public bool pursueComplete = false;

    public GuardPursueBehavior(Guards  guard,Transform targetObject, float pursueMoveSpeed)
    {
        this.guards = guard;
        this.targetObject = targetObject;
        this.pursueMoveSpeed = pursueMoveSpeed;
        this.attackRotateSpeed = guards.generalData.attackRotateSpeed;
        guardAnimator = guards.GetComponent<Animator>();
        this.visionData = guards.visionData;
        meshAgent = guards.GetComponent<NavMeshAgent>();
        //targetPreviousPosition = targetObject.position;
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        float distanceToTarget = Vector3.Distance(meshAgent.transform.position, targetObject.position);
        if(distanceToTarget <= visionData.attackRange || playerInRange)
        {
            playerInRange = true;
            meshAgent.isStopped = true;
            meshAgent.updateRotation = false;
            meshAgent.velocity = Vector3.zero;

            //visionData.radius = 20;
            //visionData.awarnessZone = 20;
            
            //Do not use this (LookAt) bcoz it suddenly looks at the player without any Rotation or Time interval.
            //guards.transform.LookAt(targetObject.transform, Vector3.up);  
            RotateTowardsTarget();

            guardAnimator.SetTrigger("Attack");
        }
        else
        {
            meshAgent.isStopped = false;
            meshAgent.updateRotation = true;
            //meshAgent.SetDestination(targetObject.transform.position);    //Moves the Guard towards the Player.
            meshAgent.SetDestination(PredictFuturePosition());  //Predicting using Player's Position.
        }

        if(distanceToTarget >= visionData.searchRange)
        {
            playerInRange = false;
            //guardController?.SetLookAroundBehavior();
            //visionData.radius = 5;
            //visionData.awarnessZone = 5;
        }

        targetPreviousPosition = targetObject.position; 

        pursueComplete = false;

    }

    private Vector3 PredictFuturePosition()
    {
        Vector3 targetCurrentPosition = targetObject.position;

        if(Time.deltaTime == 0)
        {
            return targetCurrentPosition;
        }

        //The prediction is done by getting Player's Movement (in position X, Y & Z co-ordinates)
        //i.e., veclocity = currentPosition - previousPosition / Time.deltaTime
        //and then (prediction) futurePos =  currentPosition + veclcity
        //Calculation of prediction
        Vector3 targetVelocity = (targetCurrentPosition - targetPreviousPosition) / Time.deltaTime;
        Vector3 futurePos = targetCurrentPosition + targetVelocity;

            //This is Extra Code to draw line for visualization purpose
            Vector3 end = new Vector3(futurePos.x, futurePos.y, futurePos.z + 5);
            Debug.DrawLine(futurePos, end, Color.blue);

        return futurePos;

        //Note: FuturePos is always a Position Value Greater than where we are moving.

    }

    private void RotateTowardsTarget()
    {
        Vector3 goalDirection = targetObject.position - meshAgent.transform.position;
        goalDirection.y = 0;
        goalDirection = goalDirection.normalized;
        float stepAmount = attackRotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(guards.transform.forward, goalDirection, stepAmount, 0.0f);
        meshAgent.transform.forward = newDirection;

    }

    public override void End()
    {
        meshAgent.isStopped = false;
        meshAgent.updateRotation = true;
        pursueComplete = true;
    }
}
