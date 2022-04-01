using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EndLevelInteractionBehaviour : InteractionBehaviour
{
	public EndLevelInteractionBehaviour(EndLevelInteraction data)
	{
		UpdateMonitorVisuals(data);
		UpdateVFX(data);
	}

	private void UpdateMonitorVisuals(EndLevelInteraction data)
	{
		Material material = data.monitorRenderer.materials[data.materialIndex];
		material.SetTexture("_MainTex", data.unlocked);
		material.SetTexture("_EmissionMap", data.unlocked);
	}


	private void UpdateVFX(EndLevelInteraction data)
	{
		data.unlockedVFX.SetActive(true);
	}

	public override bool CanInteract()
	{
		return true;		
	}
}
