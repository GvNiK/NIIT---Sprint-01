using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolWait : PatrolCommand     //Assignment - 03
{
    private float waitDuration;
    private float timeWaited = 0.0f;

    public PatrolWait(float waitDurations)   //This will be accessed to put the 'wait for seconds' time value.
    {
        this.waitDuration = waitDurations;
    }
    public override void Start()
    {
        
    }

    public override void Update()
    {
        timeWaited += Time.deltaTime;   

        if(timeWaited >= waitDuration)  //Checks the 'waitDuration' i.e., "Wait" time entered in Inspector.
        {
            CommandComplete();
        }
    }
    public override void End()
    {
        //Debug.Log("and Waited for : " + "<color=red>" + timeWaited + "</color>" + " seconds.");
    }
}
