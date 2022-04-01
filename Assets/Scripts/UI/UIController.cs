using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController 
{
	private HUDController hudController;
    private LevelEndMenuController levelEndMenu;
	private Player player;
	private TimeController timeController; 	//Assignment - 01
	private LevelIntroUIController levelIntroController;
	private InventoryMenuController inventoryMenuController;	//Assignment - 04 - Part II
	//private InventoryController inventoryController;	//Assignment - 04 - Part II
	private int currentLevelID;
	private PauseMenuController pauseMenuController;	//Assignment - 01
	public Action<Levels.Data> OnLevelLoad = delegate { };	//Assignment - 02 (Conatins <Levels.Data> bcoz to load level)
	public Action OnExit = delegate { };	//Assignment - 02

	public UIController(Player player,	//Constructor
		Transform cameraTransform, int currentLevelID, TimeController timeControl, LevelStatsController levelStatsController, //Assignment - 01
		GameObject inventoryUI, InventoryController inventoryController, PickupEvents pickupEvents)	//Assignment - 04 - Part II & S2 - Assignment 02
    {
		this.player = player;
		this.currentLevelID = currentLevelID;
		this.timeController = timeControl;	//Assignment - 01
		//this.inventoryController = inventoryController;

        hudController = cameraTransform.Find("HUDCanvas/HUD").GetComponent<HUDController>();
        hudController.SetupHUDController(player, inventoryController, pickupEvents);	//S2 - Assignment 02

		levelEndMenu = new LevelEndMenuController(hudController.GetComponentInChildren<LevelEndMenu>(true), currentLevelID, levelStatsController);
		
		levelIntroController = new LevelIntroUIController(hudController.transform, player.Broadcaster, currentLevelID);

		pauseMenuController = hudController.GetComponentInChildren<PauseMenuController>();	//Assignment - 01 (as 'Pause' in Child to 'HUD'')

		player.Broadcaster.Callbacks.OnPlayerPauseRequested += () => ShowPause();	//Assignment - 01
	
		player.Broadcaster.Callbacks.OnPlayerResumeRequested += () => ShowResume();	//Assignment - 01
		
		pauseMenuController.OnResume += () => { ShowResume(); }; 	//Assignment - 01 (for 'Resume' button - to Resume)

		pauseMenuController.OnPause += () =>	{ ShowPause(); };	//Assignment - 01  (for 'PauseIcon' button - to Pause)

		pauseMenuController.OnRestart += () =>	//Assignment - 02 (for 'RestartButton' - to Restart)
		{
			OnLevelLoad(GetCurrentLevel());
			ResumeGame();
		}; 

		levelEndMenu.OnLevelLoadRequested += (level) =>		//Assignment - 02 (to 'LevelController')
		{
			OnLevelLoad(level);
			ResumeGame();
		};

		levelEndMenu.OnExitRequested += () =>		//Assignment - 02 (to 'LevelController')
		{
			OnExit(); 
			ResumeGame();
		};

		levelEndMenu.OnRetryRequested += () =>		//Assignment - 02 (to 'LevelController')
		{
			OnLevelLoad(GetCurrentLevel());
			ResumeGame();
		};	

		inventoryMenuController = new InventoryMenuController(hudController.transform, inventoryController, inventoryUI, player.playerEquipment);	//Assignment 04 - Part II

		player.Broadcaster.Callbacks.OnInventoryRequested += () => ShowInventory();
		hudController.OnInventoryRequested += ShowInventory;	//Also can be written as: += () => ShowInventory();

		inventoryMenuController.OnClose += () =>	//The "X" icon on the Inventory Menu UI.
		{
			//inventoryMenuController.Hide(); No need for this bcoz we have added "OnClose()" delegate in Hide().
			player.Broadcaster.EnableActions(ControlType.Gameplay);
			hudController.ShowHUD();
		};

	}

    private Levels.Data GetCurrentLevel()		//Assignment - 02 (Logic to get Current Level)
    {
        foreach(Levels.Data level in Levels.ALL)
		{
			if(level.ID == currentLevelID)
			{
				return level;
			}
		}
		
		return null;
    }

    private void ShowResume()	//Assignment - 01 
    {
		ResumeGame(); //Timescale = 1;

		//GameIsPaused = false; 
		hudController.ShowHUD();
		pauseMenuController.Hide();
    } 


    private void ShowPause()	//Assignment - 01
    {
		PauseGame(); //Timescale = 0;

		//GameIsPaused = true; 
		hudController.HideHUD();
		pauseMenuController.Show();
    }
	private void ShowInventory()	//Assignment 04 - Part II
	{
		inventoryMenuController.Show();
		player.Broadcaster.EnableActions(ControlType.None);
		hudController.HideHUD();

	}

    public void OnLevelFailed(string message)
	{
		levelEndMenu.Show(message, false);
		PauseGame();	//Assignment - 01 (Stops Time & Disables Inputs)
	}

	public void OnLevelComplete(string message)
	{
		levelEndMenu.Show(message, true); 
		PauseGame();	//Assignment - 01 (Stops Time & Disables Inputs)
	}

	public void Update()
	{
		levelIntroController.Update();
	}

	public void PauseGame()	//Assignment - 01
	{
		timeController.StopTime();
		//player.Broadcaster.EnableActions(ControlType.None);
		player.Broadcaster.EnableActions(ControlType.Menu);		//Assignment - 01 This enables the Keyboard Input for "Pause" as we have set Off the "ControlType" to "None" Inputs.
	}
	public void ResumeGame()	//Assignment - 01
	{
		timeController.StartTime();
		player.Broadcaster.EnableActions(ControlType.Gameplay);
	}
}
