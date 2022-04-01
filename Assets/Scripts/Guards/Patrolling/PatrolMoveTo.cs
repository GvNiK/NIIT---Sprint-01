using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PatrolMoveTo : PatrolCommand   //Assignment - 03
{
    private NavMeshAgent meshAgent;
    private Transform goal;

    //Constructor - Passing of Data from One Script to Another.
    public PatrolMoveTo(NavMeshAgent meshAgent, Transform goal) //Bcoz this will be called in GuardController Script.
    {
        this.meshAgent = meshAgent;
        this.goal = goal;
    }

    public override void Start()
    {
        meshAgent.SetDestination(goal.position); //Same as wayPoint.positin;
    }

   
    public override void Update() //Moves the Guard
    {
        float distanceToGoal = (meshAgent.destination - meshAgent.transform.position).magnitude;
        if(!meshAgent.hasPath || distanceToGoal < 0.2f)    
        {
            //meshAgent.ResetPath();
            CommandComplete(); //Calls 'Event Action' delegate function.
        }
    }

    public override void End()
    {
        //Debug.Log( meshAgent + " Moved to : " + "<color=orange>" + goal +  "</color>" + ".");
    }
}
