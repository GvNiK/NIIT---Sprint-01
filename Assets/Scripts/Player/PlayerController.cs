using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController
{
	public Action<float> OnPlayerDamageTaken = delegate { };
	public Action OnDeathSequenceStarted = delegate { };
	public Action OnDeathSequenceCompleted = delegate { };
	public Action OnInteractionAvailable = delegate { };
	public Action OnAvailableInteractionLost = delegate { };
	public Action<ItemType> OnEquipmentUsed = delegate { };		//Extra GuardDamage
	public Action<Guards, ItemType, Vector3> OnEquipmentCollidedWithGuard = delegate { };	//Extra GuardDamage
	public Action<ItemType, Vector3> OnEquipmentCollidedWithEnvironment = delegate { };	//Extra
	private Transform transform;

	public PlayerHealth playerHealth;	//S2 - Assignment 02

	private NavMeshAgent navMeshAgent;
	private Rigidbody rigidbody;

	private PlayerObjectData playerObjectData;
	private PlayerInteractionController interactionController;
	private AnimationListener animationListener;
	private Vector3 lastDamageLocation;
	private Tween currentTween;
	private PlayerMovementController movementController;
	private PlayerCollision collision;
	private PlayerInputBroadcaster inputBroadcaster;
    private Animator animator;	//Sw - Assignment 02
    private PlayerViewRelativeMovement viewRelativeMovement;
	
	private PlayerInputCallbacks callbacks;
	private PlayerEquipmentController equipmentController;	//Assignment 04 - Part I
	private InventoryController inventory;	//Assignment 04 - Part II
	public ProjectilePool projectilePool;	//Assignment 04 - Part II
	private PlayerAnimationController playerAnimationController;	//S2 - Assignment 01
	private float moveSpeedMultiplayer = 1.5f;	//S2 - Assignment 01 

	private PlayerSettings settings;

	public PlayerController(Transform transform,
		NavMeshAgent navMeshAgent, PlayerInteractionController interactionController, 
		PlayerCollision collision, PlayerInputBroadcaster inputBroadcaster, Animator animator, PlayerEvents playerEvents,	//S2 - Assignment 01
		InventoryController inventory, PlayerEquipmentController equipmentController, ProjectilePool projectilePool, PlayerSettings settings)	//Assignment 04 - Part I & II
    {
		this.transform = transform;
		this.navMeshAgent = navMeshAgent;
		this.interactionController = interactionController;
		this.collision = collision;
		this.inputBroadcaster = inputBroadcaster;
        this.animator = animator;
        this.equipmentController = equipmentController;		//Assignment 04 - Part I
		this.inventory = inventory;	//Assignment 04 - Part I
		this.projectilePool = projectilePool; 	//Assignment 04 - Part II
		this.settings = settings;

		playerHealth = new PlayerHealth(settings.playerMaxHP);

		playerHealth.OnDamageTaken += (currentHealth) =>
		{
            TakeHit(lastDamageLocation);	//S2 - Assignment 02
			OnPlayerDamageTaken(currentHealth);
		};

		playerHealth.OnKilled += () =>
		{
			OnDeathSequenceStarted();
			HandlePlayerDeath();
			OnDeathSequenceCompleted();
		};

		playerEvents.OnShotTargetSet += (target, onComplete) =>		//S2 - Assignment 02
		{
			Face(target, onComplete);
		};	

		equipmentController.OnEquipmentUsed += (type) => OnEquipmentUsed(type);	//Extra GuardDamage
		equipmentController.OnCollidedWithGuard += (guard, type, collisionPos) => OnEquipmentCollidedWithGuard(guard, type, collisionPos);	//Extra GuardDamage
		equipmentController.OnCollidedwithEnvironment += (type, collisionPos) => OnEquipmentCollidedWithEnvironment(type, collisionPos);	//Extra

		rigidbody = transform.GetComponent<Rigidbody>();

		playerAnimationController = transform.Find("Human").GetComponent<PlayerAnimationController>();	//S2 - Assignment 01
		
		playerObjectData = transform.GetComponent<PlayerObjectData>();

		navMeshAgent.updateRotation = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

		//S2 - Assignment 01
		SetupAnimationListener();	
		
		playerAnimationController.Setup(animator, moveSpeedMultiplayer, rigidbody);	

		movementController = new PlayerMovementController(transform, animator);	//S2 - Assignment 01
		
		viewRelativeMovement = new PlayerViewRelativeMovement(movementController, inputBroadcaster.Callbacks);

		interactionController.OnInteractionAvailable += () => OnInteractionAvailable();
		interactionController.OnAvailableInteractionLost += () => OnAvailableInteractionLost();

	}

    public void OnPickupCollected(Pickup pickup)	//Assignment - 04 Part I
    {
        equipmentController.OnPlayerPickedUp(pickup);
    }

    public void TakeDamage(float damageAmount, Vector3 damageLocation)
	{
		lastDamageLocation = damageLocation;
		playerHealth.TakeDamage(damageAmount);
	}

	public void Update(Vector3 viewForward)
	{
		if(currentTween != null)
		{
			currentTween.Update();
		}
		viewRelativeMovement.Update(viewForward);
		equipmentController.Update();
		movementController.Update();	//S2 - Assignment 01
	}

	private void SetupAnimationListener()	//S2 - Assignment 01
	{
		animationListener = transform.Find("Human").GetComponent<AnimationListener>();
		
		animationListener.OnWeightedAnimationEvent += (arguent, weight) =>
		{
			switch(arguent)
			{
				case "DeathAnimationComplete":
					DeathAnimationComplete();
					break;
			}
		};
	}

	private void DeathAnimationComplete()
	{
		currentTween = new WaitForSeconds(2f);
		currentTween.OnComplete += () =>
		{
			currentTween = null;
			OnDeathSequenceCompleted();
		};
	}

	private bool IsPlayerFacing(Vector3 location)
	{
		float dot = Vector3.Dot(transform.forward, (location - transform.position).normalized);
		if(dot > 0.5f)
		{
			return true;
		}

		return false;
	}

	public void TakeHit(Vector3 location)	//S2 - Assignment 02
	{
		if(IsPlayerFacing(location))
		{
			animator.SetTrigger("HitFront");
		}
		else
		{
			animator.SetTrigger("HitBack");
		}
	}

	private void HandlePlayerDeath()
	{
		inputBroadcaster.EnableActions(ControlType.None);
		//DeathAnimationComplete();

		if(IsPlayerFacing(lastDamageLocation))
		{
			animator.SetTrigger("DeathFront");
		}
		else
		{
			animator.SetTrigger("DeathBack");
		}
	}

	public void Face(Vector3 target, Action OnComplete)
	{
		movementController.Face(target - transform.position, () => OnComplete());
	}

	public void Warp(Vector3 position)
	{
        navMeshAgent.Warp(position);
	}

	public void StartUse()
	{
		if (interactionController.CanInteract())
		{
			interactionController.Interact();
		}
			equipmentController.StartUse();
	}

	public bool OwnsCollider(Collider collider)
	{
		return collision.OwnsCollider(collider);
	}
	public Vector3 Position
    {
        get
		{
            return transform.position;
		}
    }

	public Transform Transform
	{
		get
		{
			return transform;
		}
	}

    public Vector3 Forward
    {
        get
        {
            return transform.forward;
        }
    }

	public PlayerObjectData Objects
	{
		get
		{
			return playerObjectData;
		}
	}
}
