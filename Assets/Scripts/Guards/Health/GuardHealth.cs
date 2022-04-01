using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardHealth    //Assignment - 03 Part I
{
    public Action<float, float> OnDamageTaken = delegate { };
    public Action OnKilled = delegate { };
    private float maxHealth;
    public float currentHealth;
    public  GuardHealth(float maxHealth)
    {
        this.currentHealth = maxHealth;
        this.maxHealth = maxHealth;
    }

    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;

        OnDamageTaken.Invoke(currentHealth, maxHealth);

        if(currentHealth <= 0)
        {
            OnKilled.Invoke();
            currentHealth = 0;
        }
    }

    public bool IsAlive
    {
        get
        {
            return currentHealth > 0;
        }
    }
}
