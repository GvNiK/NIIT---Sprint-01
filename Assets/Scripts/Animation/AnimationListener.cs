using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This Script is used to get the Animation Event that happens or has happened, on an Animation Clip or State.
public class AnimationListener : MonoBehaviour
{
	public Action<string> OnAnimationEvent = delegate { };
	public Action<string, float> OnWeightedAnimationEvent = delegate { };
    
	public void ReceiveAnimationEvent(AnimationEvent evt)
	{
		OnAnimationEvent(evt.stringParameter);
		OnWeightedAnimationEvent(evt.stringParameter, evt.animatorClipInfo.weight);
	}
}
