using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController     //Assignment - 03
{
    private GuardBehavior currentBehavior;
    private Guards guards;
    private GuardVision vision;
    private GuardHealth guardHealth;
    private GuardSuspicion guardSuspicion;  //Part II
    private NavMeshAgent meshAgent;
    private CurrentGuardState currentState;
    private Player player;
    private Animator animator;
    private GuardAnimator guardAnimator;    //Part II
    private EmoteController guardEmoteController;   //Part II
    //private IProjectile projectile;
    private ProjectilePool projectilePool;
    private GuardEvents guardEvents;    //Extra
	public Action<Guards, float, float> OnDamageTakenEvent = delegate { };    //Extra

    //private ProjectileType projectileType;

    public GuardController(Guards guard, Player player, GuardEvents guardEvents)  //Bcoz this will be called in GuardManager.  
    {
        this.guards = guard;
        this.player = player;
        this.guardEvents = guardEvents;
        meshAgent = guard.GetComponent<NavMeshAgent>();
        animator = guards.GetComponent<Animator>();

        guardAnimator = new GuardAnimator(meshAgent, animator); //Part II

        vision = new GuardVision(guard, guard.visionData, player);

        //projectile = player.Controller.projectilePool.Get(projectileType);

        //vision.OnObjectsInView += ObjectsInVision;
        //vision.OnNoObjectsInView += NoObjectsInVision;

        SetPatrolBehavior();

        guardHealth = new GuardHealth(guards.generalData.maxHealth);
        guardHealth.OnDamageTaken += GuardDamaged;
        guardHealth.OnKilled += GuardKilled;

		this.OnDamageTakenEvent += (damagedGuard, health, maxHealth) => guardEvents.Hit(damagedGuard, health, maxHealth);     //Extra

        guard.OnDamageTaken += (damageAmount,damageSource) => guardHealth.TakeDamage(damageAmount);

        guardSuspicion = new GuardSuspicion(vision, guardHealth,guards.suspicionData, guard);  //Part II
        guardSuspicion.OnSuspicionStateUpdated += UpdateSuspicionState; //Part II

        guardEmoteController = new EmoteController(guardSuspicion, guard.transform.Find("GuardEmotes").gameObject); //Part II

    }

    private void UpdateSuspicionState(CurrentGuardState newState, CurrentGuardState oldState, Transform instigator)   //Part II
    {
        currentBehavior.End();
        if(guardHealth.IsAlive)
        {
            switch(newState)
            {
                case CurrentGuardState.Patrolling: 
                    currentBehavior = new GuardPatrolBehavior(guards, guards.generalData.patrolMoveSpeed, guardAnimator);
                break;

                case CurrentGuardState.Pursuing: 
                    currentBehavior = new GuardPursueBehavior(guards, player.ObjectData.transform, guards.generalData.pursuitMoveSpeed);
                break;

                case CurrentGuardState.Annoyed: 
                    //currentBehavior = new GuardAnnoyedBehvior(guards, meshAgent, animator);
                    SetLookAroundBehavior();
                    Debug.Log("Annoying! in Switch Case");

                break;                

                default:
                    Debug.Log("Missing case in GuardController UpdateSuspicion");
                break;
            }

            currentBehavior.Start();
        }
    }

    private void GuardDamaged(float damageAmount, float maxHealth)
    {
        damageAmount = Mathf.Clamp(damageAmount, 0, maxHealth);
        animator.SetBool("TakeHitFront", true);
        Debug.Log(guards.gameObject.name + " damaged for " + damageAmount + " Max Health " + maxHealth);
    }

    // Update is called once per frame
    public void Update()
    {
        vision.Update();    //Contains the View Cone & Detection Logic.
        currentBehavior.Update();   //From GuardBehavior Script. (Command Pattern)
        guardAnimator.Update();     //As we need to Update Guard's Movement every frame. Update calls "Move()" function. //Part II
    }

    /*private void ObjectsInVision(List<GameObject> objectsInView)
    {           
        SetNewState(CurrentGuardState.Pursuing);    //Sets the "newState" to Pursuing.
    }

    private void NoObjectsInVision()
    {
        SetNewState(CurrentGuardState.Patrolling);  //Sets the "newState" to Patrolling.
    }

    private void SetNewState(CurrentGuardState newState)
    {
        if(currentState == newState)    //IMP: This line loops the Patrolling action.
        {
            return;
        }
        switch(newState)
        {
            case CurrentGuardState.Patrolling:  //Called from the above line.
 
                //The below line is IMP bcozin here we set the currentState to some state,
                //so as it can execute-break-execute seamlessly
                currentState = CurrentGuardState.Patrolling;    
                SetPatrolBehavior();
            break;

            case CurrentGuardState.Pursuing:
                //The below Line...  same as the above.
                currentState = CurrentGuardState.Pursuing;
                SetPursueBehavior();
            break;

        }
    }*/
    
    private void SetPatrolBehavior()
    {
        if(currentBehavior != null)
            currentBehavior.End();
        currentBehavior = new GuardPatrolBehavior(guards, guards.generalData.patrolMoveSpeed, guardAnimator);
        currentBehavior.Start();
    }

    public void SetLookAroundBehavior()
    {
        if(currentBehavior != null)
            currentBehavior.End();
        currentBehavior = new GuardAnnoyedBehvior(guards, meshAgent, animator);
        currentBehavior.Start();
        Debug.Log("Annoying! in SetLook function");
    }

    /*private void SetPursueBehavior()
    {
        if(currentBehavior != null)
            currentBehavior.End();
        currentBehavior = new GuardPursueBehavior(guards, player.ObjectData.transform, guards.generalData.pursuitMoveSpeed);
        currentBehavior.Start();
    }*/   

     private void GuardKilled()
    {
        Debug.Log("Guard Killed!");
        currentBehavior.End();
        currentBehavior = new GaurdDeathBehavior(meshAgent, animator, vision);
        currentBehavior.Start();
        guardHealth.OnDamageTaken -= GuardDamaged;  //No Damage taken
        guardHealth.OnKilled -= GuardKilled;
    }

    public bool CanBeTargeted //S2 - Assignment 02
    {
        //return (currentBehavior is GaurdDeathBehavior) == false; 
        get
		{
			return (currentBehavior is GaurdDeathBehavior) == false; // &&
				//(currentBehaviour is GuardDespawnBehaviour) == false;
		}
    }
}

/*public enum CurrentGuardState
{
    Patrolling,
    Pursuing
}*/

