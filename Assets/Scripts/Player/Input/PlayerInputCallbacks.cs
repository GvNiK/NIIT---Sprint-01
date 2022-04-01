using System;
using UnityEngine;

public class PlayerInputCallbacks
{
	public Action<Vector2> OnPlayerMoveFired = delegate { };
	public Action<Vector2> OnPlayerLookFired = delegate { };

	public Action OnPlayerTapFired = delegate { };
	public Action OnPlayerTapReleased = delegate { };

	public Action OnPlayerStartUseFired = delegate { };
	public Action OnPlayerEndUseFired = delegate { };
	
	public Action OnPlayerPauseRequested = delegate { };	//Assignment - 01
	public Action OnPlayerResumeRequested = delegate { };	//Assignment - 01
	public Action OnPlayerRequestedJump = delegate { Debug.Log("Jump!"); }; //Assignment - 01

    public Action OnInventoryRequested = delegate { };	//Assignment 04 - Part II
	
}
