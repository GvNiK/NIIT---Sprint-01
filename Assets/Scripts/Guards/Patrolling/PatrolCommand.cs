using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatrolCommand      //Assignment - 03
{
    public abstract void Start();
  
    public abstract void Update();

    public abstract void End();

    public event Action OnCommandComplete;

    protected virtual void CommandComplete()
    {
        OnCommandComplete?.Invoke();
    }
 
}
