using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SwordAttackAnimation
{
	private const float ANIM_TIME = 0.3f;
	public Action OnAttackStarted = delegate { };
	public Action OnAttackEnded = delegate { };
	private Vector3 returnPos;
	private Vector3 lungePos;
	private Transform sword;
	private bool active;	//Default: false
	private float remainingAnimTime;

	public SwordAttackAnimation(Transform sword)
	{
		this.sword = sword;

		returnPos = sword.localPosition;
		lungePos = returnPos + new Vector3(0, 0, 1f);	//Moves Sword One Unit Forward (in Z-Axis).
	}
	public void Start()
	{
		active = true;
		remainingAnimTime = ANIM_TIME;
		OnAttackStarted();
	}

	public void Update()
	{
		if(remainingAnimTime >= 0)
		{
			remainingAnimTime -= Time.deltaTime;

			if(remainingAnimTime <= 0)
			{
				active = false;
				sword.localPosition = returnPos;
				OnAttackEnded();
				return;
			}

			//This is the Value(in float, but which is taken as Percentage) to which the Lerp() will blend from "returnPos" & "lungePos" values.
			float normalizedAnimTime = (ANIM_TIME - remainingAnimTime) / ANIM_TIME;	
			if(remainingAnimTime > (ANIM_TIME / 2f))
			{
				//We use "Lerp()" function bcoz we want to transition the movement of the Sword, from initial position to forward position.
				//If we do not use Lerp(), then there will be direct or sudden displacement between the positions of the Sowrd.
				sword.localPosition = Vector3.Lerp(returnPos, lungePos, normalizedAnimTime * 2f);
			}
			else
			{
				//Here, we switch the lungPos & returnPos as swe are going back from "Forward Position" to "Original Position".
				sword.localPosition = Vector3.Lerp(lungePos, returnPos, (normalizedAnimTime - 0.5f) * 2f);
			}
		}
	}

	public bool IsActive
	{
		get { return active; }
	}

}
