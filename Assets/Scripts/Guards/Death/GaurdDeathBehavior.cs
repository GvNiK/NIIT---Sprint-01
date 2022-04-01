using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GaurdDeathBehavior : GuardBehavior
{
    private NavMeshAgent meshAgent;
    private Animator animator;
    private GuardVision vision;
    private float currentTime;
    private float waitTime = 5.0f;

    public GaurdDeathBehavior(NavMeshAgent meshAgent, Animator animator, GuardVision vision)
    {
        this.meshAgent = meshAgent;
        this.animator = animator;
        this.vision = vision;
    }
    public override void Start()
    {
        animator.SetBool("Dead", true);
        meshAgent.ResetPath();
        meshAgent.isStopped = true;
        meshAgent.updateRotation = false;
        meshAgent.velocity = Vector3.zero;
        meshAgent.GetComponent<BoxCollider>().enabled = false;  //GetComponentInChildren also works
        meshAgent.GetComponent<NavMeshAgent>().enabled = false;
        vision.Disable();
        Debug.Log("<b>Guard's Body will Fade Out in 5 Seconds.</b>");
        
    }

    public override void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= waitTime)
        {
            animator.SetBool("Dead_Fade", true);

            //Fade Out Delay - Need to set it bcoz, if we set to to just "waitTime" i.e.(5 seconds)
            //then it disables the Guard GameObject before playing the "Dead_Fade" animation.
            //So to disable it only after the Fade animation is played, we need to add delay of some time seconds.
            //Note: The Time values should always be above 2, bcoz it takes 1 second to complete the Fade animation.
            if(currentTime >= waitTime + 2) 
                meshAgent.gameObject.SetActive(false);   //Big Savior
        }     
    }

    public override void End()
    {
        //To bring the guard back to life
        animator.SetBool("Dead", false);
        meshAgent.isStopped = false;
        meshAgent.updateRotation = true;
        meshAgent.GetComponent<BoxCollider>().enabled = true;
        meshAgent.GetComponent<NavMeshAgent>().enabled = true;
        vision.Enable();
    }

}
