using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteController    //Part II
{
    GuardSuspicion guarSuspicion;
    GuardEmote emotes;
    //Guards[] guards;
    //Player player;
 
    public EmoteController(GuardSuspicion guardSuspicion, GameObject emotesObject)
    {
        this.guarSuspicion = guardSuspicion;
        guardSuspicion.OnSuspicionStateUpdated += (newState, oldState, intigator) => EmoteUpdated(newState, oldState);
        this.emotes = emotesObject.GetComponent<GuardEmote>();

        //for(int i = 0; i < guards.Length; i++)
            //emotesObject.transform.LookAt(player.Controller.Transform.forward, Vector3.up);
    }

    private void EmoteUpdated(CurrentGuardState newState, CurrentGuardState oldState)
    {
        switch(newState)
        {
            case CurrentGuardState.Patrolling:
            //Check whether if we are not in the Old State or our previous state was not the same as New State(i.e., already not in Patrolling State)
            if(oldState != CurrentGuardState.Patrolling)    
            {
                emotes.ShowPlayerLostEmote();
            }
            break;

            case CurrentGuardState.Pursuing:
            //Same as above
            if(oldState != CurrentGuardState.Pursuing)
            {
                emotes.ShowAlertedEmote();
            }
            break;  

            /*case CurrentGuardState.Annoyed:
            for(int i = 0; i < guards.Length; i++)
            {
                guards[i].OnDamageTaken += (damageAmount, instigator) => emotes.ShowSuspiciousEmote();
            }
            break;*/              

            default:
                Debug.LogError("Missing Case in EmoteController > EmoteUpdated");
            break;
        }
    }
}
