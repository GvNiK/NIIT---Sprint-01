using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAnnoyedBehvior : GuardBehavior
{
    private Guards guards;
    private NavMeshAgent meshAgent;
    private Animator animator;
    private bool finishedSearch = false;
    private GuardPursueBehavior pursueBehavior;

    public GuardAnnoyedBehvior(Guards guards, NavMeshAgent meshAgent, Animator animator)
    {
        this.guards = guards;
        this.meshAgent = meshAgent;
        this.animator = animator;
        pursueBehavior = guards.GetComponent<GuardPursueBehavior>();
    }

    public override void Start()
    {
        meshAgent.velocity = Vector3.zero;
        //meshAgent.speed = 0;
        //meshAgent.updatePosition = false;
        meshAgent.updateRotation = false;

        animator.SetTrigger("LookAround");
        finishedSearch = true;

        Debug.Log("LookAround Started!");
    }

    public override void Update()
    {
        if(pursueBehavior.pursueComplete == true)
        {   
            Debug.Log("Annoying!");
        }
        if(finishedSearch)
        {
            //CommandComplete();
        }
    }

    public override void End()
    {
        //meshAgent.speed = guard.generalData.patrolMoveSpeed;
        meshAgent.updatePosition = true;
        meshAgent.updateRotation = true;

        Debug.Log("LookAround Ended!");
    }    
}

