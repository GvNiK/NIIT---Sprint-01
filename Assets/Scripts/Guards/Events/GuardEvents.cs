using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GuardEvents 
{
    private Action<Guards, float, float> onHit = delegate { };
	private Action<float, Vector3> onDamageDealt = delegate { };

    public void AddHitListener(Action<Guards, float, float> listener)
	{
		onHit += listener;
	}

	public void Hit(Guards guard, float health, float maxHealth)
	{
		onHit(guard, health, maxHealth);
	}

}
