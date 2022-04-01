using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LevelController : MonoBehaviour
{
    public int levelID;
	public Action OnLoadComplete = delegate { };
	public Action<float> OnLevelComplete = delegate { };
	public Action<Levels.Data> OnLevelLoadRequest = delegate { };	
	public Action OnExitRequest = delegate { };		
    public Action OnPickupCollected = delegate { };		

    private CameraController cameraController;
    private UIController uiController;
    public Player player;	//S2 - Assignment 02
    private GuardManager guardManager;	//Assignment - 03
	private TimeController timecontroller;	//Assignment - 01
	private LevelStatsController levelStatsController;
	//private PlayerEquipmentController equipmentController;  //Assignment 04 - Part I
    private PickupController pickupController;	//Assignment 04 - Part I
    private InventoryController inventory;	//Assignment 04 - Part I
	//private PickupEvents pickupEvents;	//Assignment 04 - Part I
	private ProjectilePool projectilePool;	//Assignment 04 - Part II
	private ProjectileManager projectileManager;	//Assignment 04 - Part II
	private PlayerSettings playerSettings;
	private Action<ItemType> OnItemConsumed = delegate { };
	
    public void Start()
    {		
		LevelDependancies dependancies = GetComponentInChildren<LevelDependancies>();
		if(dependancies == null)
		{
			Debug.LogError("Unable to find LevelDependancies. Cannot play level.");
		}

		GuardEvents guardEvents = new GuardEvents();	//Extra

		GameObject playerObj = CreatePlayerObject(dependancies.player);

		projectilePool = new ProjectilePool(dependancies.projectileLibrary, new ProjectileFactory());	//Assignment 04 - Part II
		projectileManager = new ProjectileManager(projectilePool);	//Assignment 04 - Part II
		

		//'transfrom' here simply means "this.transform".
		//We are using transform bcoz the level controller is an Empty GameObject with Transform Component.
		guardManager = new GuardManager(transform, player, guardEvents);	//Assignment - 03 

		PickupEvents pickupEvents = new PickupEvents();		//Assignment - 04 Part I

		playerSettings = playerObj.GetComponent<PlayerSettingsHolder>().playerSettings;	//S2 - Assignment 04
		
		//Part II - Added projectilePool
		player = CreatePlayer(playerObj, projectilePool, guardManager);		//Assignment - 04 Part I & S2 - Assignment 04


		pickupController =  new PickupController(transform, player.Controller, pickupEvents);	//Assignment 04 - Part I

		pickupEvents.OnPickupEventCollected += (pickup) =>	//Assignment 04 - Part I
		{
			player.Controller.OnPickupCollected(pickup);	//Inside Player Controller
		};

		player.Controller.OnDeathSequenceCompleted += () =>
		{
			FailLevel();
		};


		cameraController = new CameraController(dependancies.cameraContainer,
			transform.parent, player.Controller.Transform);

		player.Interaction.OnInteractionStarted += (interaction) =>
		{
			if(interaction is EndLevelInteraction endLevel)
			{
				EndLevel(endLevel);
			}
		};

		levelStatsController = new LevelStatsController();
		timecontroller = new TimeController(); 	//Concept: Hiding	//Assignment - 01

		HUDController hudController = cameraController.MainCameraTransform.Find("HUDCanvas/HUD").GetComponent<HUDController>();
		player.Controller.OnPlayerDamageTaken += (currentHealth) => hudController.UpdatePlayerHealth(currentHealth);

		uiController = new UIController(player, cameraController.MainCameraTransform,
			levelID, timecontroller, levelStatsController, dependancies.inventoryUI, inventory, pickupEvents);	//Assignment - 04 - Part II	& S2 - Assignment 02

		uiController.OnLevelLoad += (level) =>		//Assignment - 02 (to 'GameController')
		{
			OnLevelLoadRequest(level);
		};	

		uiController.OnExit += () =>		//Assignment - 02 (to 'GameController')
		{
			OnExitRequest();
		};

		_ = new LevelVFXController(dependancies.vfxLibrary, player.Controller, guardManager, guardEvents);	//Extra



		guardManager.player = player;	//Assignment - 03

		guardManager.OnLevelLoaded();

		OnLoadComplete();
	}

	public void OnLevelLoadResume()
	{
		timecontroller?.StartTime();
		Debug.Log("OnLevelLoadResume Called!");
	}

	private void EndLevel(EndLevelInteraction endLevel)
	{
		player.Broadcaster.EnableActions(ControlType.None);
		CompleteLevel();
	}

    public void Update()
    {
		levelStatsController.UpdateTime(Time.deltaTime);
		player.Controller.Update(cameraController.MainCameraTransform.forward);
		guardManager.Update();	//Assignment - 03
		uiController.Update();
		player.Interaction.Update();
		cameraController.Update();
	}

    public void OnDestroy()
	{
		player.Broadcaster.Destroy();
	}

	private GameObject CreatePlayerObject(GameObject playerObjectPrefab)
	{
		GameObject playerObject = GameObject.Instantiate(playerObjectPrefab, transform);
		playerObject.name = "Player";

		return playerObject;
	}

    private Player CreatePlayer(GameObject playerObject, ProjectilePool projectilePool, GuardManager guardManager)	//S2 - Assignment 04
    {
		PlayerEvents playerEvents = new PlayerEvents();	//S2 - Assignment 02

        NavMeshAgent navMeshAgent = playerObject.GetComponent<NavMeshAgent>();

        PlayerObjectData objectData = playerObject.GetComponent<PlayerObjectData>();

		//Bcoz "Human" is One of the Child Object to "Player".
		Animator animator = playerObject.transform.Find("Human").GetComponent<Animator>();	//S2 - Assignment 01

		PlayerCollision collision = new PlayerCollision(playerObject.transform);

		PlayerInteractionController interaction = new PlayerInteractionController(transform, playerObject.transform, collision, objectData);

		PlayerInputBroadcaster broadcaster = new PlayerInputBroadcaster();

		
		GunTargetLocator gunTargetLocator = new GunTargetLocator(guardManager, objectData, playerSettings);	//S2 - Assignment 02 & 04

		inventory = new InventoryController();	//Assignment 04 - Part I

		//We aslo get ProjectilePool & ProjectileFactory here.
		PlayerEquipmentController equipmentController = new PlayerEquipmentController(playerObject.transform, inventory, objectData, projectilePool, broadcaster, gunTargetLocator, playerEvents);	//Assignment 04 - Part I & S2 - Assignment 02

		equipmentController.OnItemConsumed += (item) => OnItemConsumed(item);

        PlayerController controller = new PlayerController(playerObject.transform, navMeshAgent,
			interaction, collision, broadcaster, animator, playerEvents, inventory, equipmentController, projectilePool, playerSettings);	//Assignment 04 - Part I & S2 - Assugnment 02

		broadcaster.Callbacks.OnPlayerStartUseFired += () => controller.StartUse();

		return new Player(controller, broadcaster, 
			objectData, interaction, equipmentController);	//Assignment 04 - Part II
	}

	private void FailLevel()
	{
		player.Broadcaster.EnableActions(ControlType.None);
		uiController.OnLevelFailed("You were killed!");
	}

    private void CompleteLevel()
	{
		player.Broadcaster.EnableActions(ControlType.None);
        uiController.OnLevelComplete("Level Complete");
		OnLevelComplete(levelStatsController.LevelTime.Value);
	}
}

public class Player
{
    public PlayerController Controller { get; }
    public PlayerInputBroadcaster Broadcaster { get; }
	public Animator Animator { get; }
	public PlayerObjectData ObjectData { get; }
	public PlayerInteractionController Interaction { get; }
	public PlayerEquipmentController playerEquipment { get; }	//Assignment 04 - Part II

	public Player(PlayerController controller, PlayerInputBroadcaster broadcaster,
		PlayerObjectData objectData, PlayerInteractionController interaction, 
		PlayerEquipmentController playerEquipment)	//Assignment 04 - Part II
    {
        this.Controller = controller;
        this.Broadcaster = broadcaster;
		this.ObjectData = objectData;
		this.Interaction = interaction;
		this.playerEquipment = playerEquipment;	//Assignment 04 - Part II
	}
}
