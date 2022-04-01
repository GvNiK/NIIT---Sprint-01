using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager    //Assignment - 03
{
    public Player player;
    private List<Guards> guards;
    private Transform levelObject;
    private GuardEvents guardEvents;
    public Dictionary<Guards, GuardController> guardDict; //Used bcoz we can Add or Find Components using Dictionary. 
    //private List<GuardController> controller;

    public GuardManager(Transform levelObject, Player player, GuardEvents guardEvents)   //Using transform as LevelController has Transform component.
    {
        this.levelObject = levelObject;
        this.player = player;
        this.guardEvents = guardEvents;

        guardDict = new Dictionary<Guards, GuardController>();
        
    }

    // Update is called once per frame
    public void Update()
    {
        if(guardDict != null)
        {
            foreach(var guardEntry in guardDict)
            {
                guardEntry.Value.Update(); //Calls Update() from 'GuardController' sccript.
            }
        }
    }

    public void OnLevelLoaded()
    {
        guards = new List<Guards>(levelObject.GetComponentsInChildren<Guards>());   //Gets every Guard in the LevelController.
        
        foreach(Guards guard in guards)
        {
            guardDict.Add(guard, new GuardController(guard, player, guardEvents));
            //controller.Add(new GuardController(guard));
            //guards.Add(new Guards());
        }
        
    }

    public IReadOnlyDictionary<Guards, GuardController> Guards  //Extra
	{
		get
		{
			return guardDict;
		}
	}
}
