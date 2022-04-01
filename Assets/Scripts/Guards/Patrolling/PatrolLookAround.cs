using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolLookAround : PatrolCommand
{
    private NavMeshAgent meshAgent;
    private Animator animator;
    private Guards guard;
    private bool finishedSearch = false;
    public PatrolLookAround(NavMeshAgent meshAgent, Animator animator, Guards guard)
    {
        this.meshAgent = meshAgent;
        this.animator = animator;
        this.guard = guard;
    }

    public override void Start()
    {
        meshAgent.velocity = Vector3.zero;
        meshAgent.speed = 0;
        //meshAgent.updatePosition = false;
        meshAgent.updateRotation = false;

        animator.SetTrigger("LookAround");
        finishedSearch = true;

        Debug.Log("LookAround Started!");
    }

    public override void Update()
    {
        if(finishedSearch)
        {
            CommandComplete();
        }
    }

    public override void End()
    {
        meshAgent.speed = guard.generalData.patrolMoveSpeed;
        meshAgent.updatePosition = true;
        meshAgent.updateRotation = true;

        Debug.Log("LookAround Ended!");
    }    
}
