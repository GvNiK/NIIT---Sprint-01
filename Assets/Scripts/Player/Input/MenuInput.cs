using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class MenuInput : ControlMapping.IMenuActions
{
	private ControlMapping controlMapping;
	private PlayerInputCallbacks callbacks;

	public MenuInput(ControlMapping controlMapping,
		PlayerInputCallbacks callbacks)
	{
		this.controlMapping = controlMapping;
		this.callbacks = callbacks;
		controlMapping.Menu.SetCallbacks(this);	//Assignment - 01
	}
	

	public void Enable()
	{
		controlMapping.Menu.Enable();	//Assignment - 01
	}

	public void Disable()
	{
		controlMapping.Menu.Disable();	//Assignment - 01
	}

    public void OnPause(InputAction.CallbackContext context)	//Assignment - 01
    {
        switch (context.phase)
		{
			case InputActionPhase.Performed:
				callbacks.OnPlayerResumeRequested.Invoke();				
				break;			
		}
    }
}
