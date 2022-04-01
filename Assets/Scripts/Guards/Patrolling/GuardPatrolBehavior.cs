using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardPatrolBehavior : GuardBehavior
{
    private Guards guards;
    private PatrolCommand currentCommand;
    private NavMeshAgent meshAgent;
    private WayPointInfo currentWaypoint; //4 Options Menu
    private int waypointIndex;
    private float patrolMoveSpeed;
    private Animator animator;
    private GuardAnimator guardAnimator;

    public GuardPatrolBehavior(Guards guard, float patrolMoveSpeed, GuardAnimator guardAnimator)
    {
        this.guards = guard;
        this.patrolMoveSpeed = patrolMoveSpeed;
        waypointIndex = 0;
        meshAgent = guard.GetComponent<NavMeshAgent>();
        animator = guard.GetComponent<Animator>();
        this.guardAnimator = guardAnimator;
        ExecuteCommand();  
    }

    public override void Start()
    {
        meshAgent.speed = patrolMoveSpeed;
        ExecuteCommand();
    }

    public override void Update()
    {
        if(guardAnimator.isInLocomotion)    //Part II
        {
            meshAgent.speed = patrolMoveSpeed;  //Part II

            if(currentCommand != null)
            {
                currentCommand.Update();    //Moves the Guard (contains code to move the guard - from 'PatrolMoveTo' script)
            }

        else    //Part II
        {
            meshAgent.speed = 0;    
        }

        }
    }

    public override void End()
    {
        
    }

     void ExecuteCommand()
    {
        //This Line just calls the WayPointInfo Class and selects the First Index Value.
        currentWaypoint = guards.waypointsList.wayPoints[waypointIndex]; //waypointList > WayPoints > WayPointInfo
       
        switch(currentWaypoint.wayPointType)    //Calls Enum
        {
            case WayPointType.MoveTo:
                //This Line actually moves the Guard by accessing the 'goal' Transfrom.
                currentCommand = new PatrolMoveTo(meshAgent, currentWaypoint.goal);

                //This Function is only executed when it gets called from "Update()" method from "PlayerMoveTo' script.
                currentCommand.OnCommandComplete += NextCommand; //Subscribing using the delegate. Called from PatrolMoveTo - 'Update' method.
                currentCommand.Start(); //Sets Guard to Initial Position (i.e. wayPoint01)
                break;

            case WayPointType.Wait:
                currentCommand = new PatrolWait(currentWaypoint.waitTime);
                currentCommand.OnCommandComplete += NextCommand; //Subscribing using the delegate. Called from PatrolMoveTo - 'Update' method.
                currentCommand.Start(); //Sets Guard to Initial Position (i.e. wayPoint01)
            break;

            case WayPointType.Rotate:
                currentCommand = new PatrolRotate(meshAgent, currentWaypoint.targetRotation, 2.0f, animator, guardAnimator);
                currentCommand.OnCommandComplete += NextCommand; 
                currentCommand.Start(); 
            break;

            case WayPointType.LookAround:
                currentCommand = new PatrolLookAround(meshAgent, animator, guards);
                currentCommand.OnCommandComplete += NextCommand; 
                currentCommand.Start(); 
            break;            

        }
    }

    private void NextCommand()  //Loops the Guard Patrol Movement
    {
        currentCommand.End();
        currentCommand.OnCommandComplete -= NextCommand;    //Garbage Code
        waypointIndex++;
        if(waypointIndex >= guards.waypointsList.wayPoints.Count)
        {
            waypointIndex = 0;
        }

        ExecuteCommand(); //Loops the Move command.
        
    }

}

