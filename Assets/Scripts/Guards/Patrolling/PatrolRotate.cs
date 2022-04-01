using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolRotate : PatrolCommand
{
    private NavMeshAgent meshAgent;
    private Vector3 rotateGoal;
    private float rotateSpeed;
    private Animator animator;
    private GuardAnimator guardAnimator;
    private Guards guards;

    public PatrolRotate(NavMeshAgent meshAgent, Vector3 rotateGoal, float rotateSpeed, Animator animator, GuardAnimator guardAnimator)
    {
        this.meshAgent = meshAgent;
        this.rotateGoal = Quaternion.Euler(rotateGoal) * Vector3.forward;   //forward - Always gives a magnitude value of 1.
        this.rotateSpeed = rotateSpeed;
        this.animator = animator;
        this.guardAnimator = guardAnimator;
    }
    public override void Start()
    {
        //Debug.Log("Rotate Started!");
        //meshAgent.speed = 0;
        meshAgent.updateRotation = false;
    }

    public override void Update()
    {
        if(rotateSpeed == 0)
        {
            Debug.Log("Did not rotate Properly!");
            CommandComplete();
        }

        float stepAmount= rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(meshAgent.transform.forward, rotateGoal, stepAmount, 0.0f);
        meshAgent.transform.forward = newDirection;
        //animator.SetFloat("StrafeSpeed", 1);

        //Returns the Signed Angle in Degrees between From and To.
        var signedAngle = Vector3.SignedAngle(rotateGoal, meshAgent.transform.forward, Vector3.up);

        //We divide it with 45 Degress to get a Scalar Amount. And aslo bcoz the Angles are in Higher values which if used anywhere 
        //will give wierd results like Rotating too Fast due to higher values. SO we need to put them down to Single Digits Lower Values.
        float rotateScaleSpeed = signedAngle / 45.0f;

        guardAnimator.TurnOnSpot(rotateScaleSpeed * rotateSpeed);

        //Used to check if the Guard is Perpendiculat to the Waypoint.
        //where, 0 means Perpendicular & 1 or above means Not Perpendicular.
        //So, in this case we check if the Dot Product is 1 or above, then it is not perpendicular and thus, end the Action by calling Complete function.
        if( Vector3.Dot(meshAgent.transform.forward, rotateGoal) > 0.99f)   
        {                                                                   
            CommandComplete();
            return;
        } 

        //navMesh.transform.Rotate(0, new Vector3(0, rotateGoal.transform.forward, 0), 0);
        //navMesh.transform.localRotation = Vector3.RotateTowards(navMesh.transform.rotation, rotateGoal.transform.rotation);
    }

    public override void End()
    {
        //Debug.Log("Rotate Ended!");
        //if(guards != null)
            //meshAgent.speed = guards.generalData.patrolMoveSpeed;
        meshAgent.updateRotation = true;
    }
}
