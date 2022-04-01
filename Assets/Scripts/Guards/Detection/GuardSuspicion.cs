using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSuspicion     //Part II
{
    private float suspicionThresold = 0.0f;
    private CurrentGuardState currentState;

    //CurrentGuardState - These is used 2 time bcoz, the 1st one represents "CurrentState" and the 2nd one represents "NextState".
    //Transform - is used to get the Transform of the Object that the Guard just saw or made contact with. 
    //("GameObject" is not used here for Optimization purpose)
    public Action<CurrentGuardState, CurrentGuardState, Transform> OnSuspicionStateUpdated = delegate { }; 
    private Transform firstObjectInView;
    private GuardHealth healthController;
    private SuspicionData suspectData;
    private GuardVision vision;
    //private IProjectile projectile; //Assignment 04 - Part II
    private Guards guards;
    private Collider guardCollider;

    public GuardSuspicion(GuardVision vision, GuardHealth healthController, SuspicionData suspicionData, Guards guard)
    {
        this.vision = vision;
        this.healthController = healthController;
        this.suspectData = suspicionData;
        vision.OnObjectsInView += BuildSuspicion;
        vision.OnNoObjectsInView += DecaySuspicion;
        currentState = CurrentGuardState.Patrolling;
        this.guards = guard;
        guard.OnDamageTaken += (damage, damageSource) => DamageTaken(damageSource, damage);

        
        guardCollider = guard.GetComponent<Collider>();
    }

    private void DamageTaken(Transform damageSource, float damage)
    {
        if(healthController.IsAlive)
        {
            suspicionThresold = 1.0f;
            OnSuspicionStateUpdated(CurrentGuardState.Pursuing, currentState, damageSource);
            currentState = CurrentGuardState.Pursuing;

            //projectile.OnCollidedWithTarget(guardCollider, guards.transform.position);
        }

    }

    private void BuildSuspicion(List<GameObject> obj)
    {
        firstObjectInView = obj[0].transform;
        suspicionThresold += suspectData.suspicionBuildRate * Time.deltaTime;   //As it says "Build" we Increment.
        suspicionThresold = Mathf.Clamp(suspicionThresold, 0.0f, 1.0f);
        CheckThresold();
    }
    
    private void DecaySuspicion()
    {
        suspicionThresold -= suspectData.suspicionDecayRate * Time.deltaTime;   //As it says "Decay" we Decrement.
        suspicionThresold = Mathf.Clamp(suspicionThresold, 0.0f, 1.0f);
        CheckThresold();
    }


    private void CheckThresold()
    {
        if(     currentState != CurrentGuardState.Pursuing &&       //currentState != CurrentGuardState.Pursuing - is used to make sure that Guard is already not in that state
                suspicionThresold >= suspectData.pursuingThresold)
            {
                    OnSuspicionStateUpdated(CurrentGuardState.Pursuing, currentState, firstObjectInView);
                    currentState = CurrentGuardState.Pursuing;
            }
        else if(currentState != CurrentGuardState.Patrolling &&     //Same reason as of above
                suspicionThresold < suspectData.patrollingThresold)
            {
                    OnSuspicionStateUpdated(CurrentGuardState.Patrolling, currentState, null);
                    currentState = CurrentGuardState.Patrolling;
            }
        else if    (currentState != CurrentGuardState.Annoyed &&
                suspicionThresold > suspectData.lookingThresold)
                {
                    OnSuspicionStateUpdated(CurrentGuardState.Annoyed, currentState, null);
                    currentState = CurrentGuardState.Annoyed;
                }
    }
}

public enum CurrentGuardState
{
    Patrolling,
    Pursuing,
    Annoyed
}
