using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController 
{
    //public Action OnTimeStopped = delegate { };
    //public Action OnTimeResumed = delegate { };
    
    public void StopTime()
    {
        Time.timeScale = 0;
        //OnTimeStopped.Invoke();
    }

    public void StartTime()
    {
        Time.timeScale = 1;
        //OnTimeResumed.Invoke();
    }
}
