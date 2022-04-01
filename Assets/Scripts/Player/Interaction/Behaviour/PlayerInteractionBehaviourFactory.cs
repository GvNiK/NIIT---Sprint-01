using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerInteractionBehaviourFactory
{
	public InteractionBehaviour Create(InteractionPoint interactionPoint)
	{
		if(interactionPoint is EndLevelInteraction endLevelInteraction)
		{
			return new EndLevelInteractionBehaviour(endLevelInteraction);
		}
		else
		{
			Debug.LogError("Interaction type not supported. No behaviour available.");
		}
		return null;
	}
}
