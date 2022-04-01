using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class PlayerInputBroadcaster 
{
	private PlayerInputCallbacks callbacks;
	private ControlMapping controlMapping;
	private GameplayInput gameplayInput;
	private MenuInput menuInput;	//Assignment - 01

	public PlayerInputBroadcaster()
	{
		callbacks = new PlayerInputCallbacks();
		controlMapping = new ControlMapping();
		gameplayInput = new GameplayInput(controlMapping, callbacks);
		menuInput = new MenuInput(controlMapping, callbacks);	//Assignment - 01
		EnableActions(ControlType.Gameplay);
	}

	public void Destroy()
	{
		controlMapping.Dispose();
	}

	public void EnableActions(ControlType controlType)
	{
		DisableActions();

		switch(controlType)
		{
			case ControlType.Gameplay:
				gameplayInput.Enable();
				break;

			case ControlType.Menu:	//Assignment = 01
				menuInput.Enable();
				break;
		}
	}

	private void DisableActions()
	{
		gameplayInput.Disable();
		menuInput.Disable();	//Assignment = 01
	}

	public PlayerInputCallbacks Callbacks
	{
		get
		{
			return callbacks;
		}
	}
}
