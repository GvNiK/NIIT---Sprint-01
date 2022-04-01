using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : IEquipableMain
{
    //public static float COOL_DOWN_PERIOD = 0.3f;
    public Action<ItemType> OnItemConsumed { get{ return toConsume; } set { toConsume = value; }}
    public Action OnUsed { get{ return toUse; } set{ toUse = value; } }
    public Action<Guards, ItemType, Vector3> OnCollidedWithGuard { get { return toConsumeGuardCollision; } set { toConsumeGuardCollision = value; } }
    public Action<ItemType, Vector3> OnCollidedwithEnvironment { get => toConsumeEnvironment; set => toConsumeEnvironment = value; }    //Extra
    public Action<Transform, ProjectileType> OnProjectileSpawned { get { return ProjectileConsumed; } set => ProjectileConsumed = value; }
   
    public ItemType Type => ItemType.Gun;

    private Action<ItemType> toConsume = delegate { };
    private Action toUse = delegate { };
    private Action<Guards, ItemType, Vector3> toConsumeGuardCollision = delegate { };    
    private Action<ItemType, Vector3> toConsumeEnvironment = delegate { };  //Extra
    private Action<Transform, ProjectileType> ProjectileConsumed= delegate { };
    //public Action<Transform, ProjectileType> OnProjectileSpwaned = delegate { };

    private float coolDown;
    private GameObject _gameObject;
    private PlayerObjectData playerObjectData;
    private ProjectilePool projectilePool;
    private IGunAmmo ammo;

    // S2 - Assignment 02 //
    private Transform bulletStart;
    private bool attackActive;  
    private Animator animator;
    private AnimationListener animationListener;

    //S2 - Assignment 02
    private PlayerInputBroadcaster inputBroadcaster;
    private GunTargetLocator gunTargetLocator;
    private PlayerEvents playerEvents;
    private Transform transform;
    private Target currentTarget;
    private Collider[] colliders;	//Extra

    public Gun(GameObject gameObject, PlayerObjectData playerObjectData, GunTargetLocator gunTargetLocator, PlayerEvents playerEvents,  //S2 - Assignment 02
                ProjectilePool projectilePool, PlayerInputBroadcaster inputBroadcaster)
    {
        this._gameObject = gameObject;
        this.playerObjectData = playerObjectData;   
        this.projectilePool = projectilePool;

        //S2 - Assignment 02
        this.inputBroadcaster = inputBroadcaster;   
        animator = playerObjectData.PlayerAnimatior;
        this.gunTargetLocator = gunTargetLocator;
        this.transform = gameObject.transform;
        this.playerEvents = playerEvents;

        bulletStart = playerObjectData.Blaster.Find("BulletStart");

        colliders = gameObject.GetComponentsInChildren<Collider>(); //Extra

        animationListener = animator.gameObject.GetComponent<AnimationListener>(); 
    }

    public void Update()
    {
        //coolDown -= Time.deltaTime;
        //Mathf.Clamp(coolDown, 0.0f, COOL_DOWN_PERIOD);  //Bcoz the coolDown value can be Negative so it clamps it to Zero.
    }

    public void Destroy()
    {
        _gameObject.SetActive(false);
    }

    public void StartUse()
    {
        //S2 - Assignment 02
        AttackStart();

        Target target = FindTarget();

        playerEvents.OnShotTargetSet.Invoke(target.position, () =>
        {
            animator.SetTrigger("Shoot");
        });
        
        currentTarget = target;
    }

    private Target FindTarget() //S2 - Assignment 01 - Part II
    {
        Transform closestEnemy = null;
        gunTargetLocator.Locate( (closest) => closestEnemy = closest );
        //Explanation of the Above Line.
        /*private LocateAction(Transform closest)
        {
            Transform closestEnemy = null;
            closestEnemy = closest;
        }*/

        if(closestEnemy != null)
        {
            Debug.Log("ClosesetEenmy");
            return new TransformTarget(closestEnemy);
        }
        else
        {
            //IMP: We cannot use "transform.position" as this is not a MonoBehaviour Script.
            //So we assign this GameObject's Transform to the Transform.
            //See Lines 38 & 55.
            Debug.Log("Null");
            return new VectorTarget(transform.position + transform.forward);    //Bcoz we want to make our Player Look at its Forward Direction.
        }
    }

    private void FireBullet()   //S2 - Assignment 02
    {   
        //TODO Shoot Ammo
        if(ammo == null)
        {
            return;
        }

        ItemType ammoType = ammo.Type;  //ammo: ProjectileType & Type: ItemType
        ProjectileType projectileType = ammo.projectileType;
        IProjectile projectile = projectilePool.Get(projectileType);
        projectile.OnSubProjectileSpawned += (subProjectile) => ProjectileSpawned(subProjectile, projectileType);
        projectile.OnCollidedWithEnvironment += (collisionPoint) => OnCollidedwithEnvironment(ammoType, collisionPoint);
        projectile.OnCollidedWithTarget += (collider, collisionPoint) => CollidedWithTarget(collider, collisionPoint);
        ProjectileSpawned(projectile.Transform, projectileType);
        //Transform blaster = playerObjectData.Blaster;

        Vector3 directionToEnemy = currentTarget.position - transform.position; //S2 - Assignment 02
        projectile.Fire(bulletStart, transform.position, directionToEnemy);    //Before: blaster.forward //S2 - Assignment 02

        OnUsed();
        OnItemConsumed(ammoType);   //ammo.Type

        //coolDown = COOL_DOWN_PERIOD;    //Decrementing
    }

    private void AttackStart()
    {
        attackActive = true;
        //FireBullet();
        inputBroadcaster.EnableActions(ControlType.None);   //Bcoz Player should Not Move while playing the Shoot Animation.
        //Debug.Log("Player inputs = None!");
    }
    
    private void AttackEnd()
    {
        attackActive = false;
        inputBroadcaster.EnableActions(ControlType.Gameplay);   //Player can Move after the Shoot Animation has ended.
        //Debug.Log("Player inputs = GamePlay!");
    }

    public void EndUse()
    {
        
    }

    public void Equip(IEquipable currentMainHandEquipment, Transform equipmentHolder)
    {
        animationListener.OnAnimationEvent += OnAnimationEvent;
        _gameObject.SetActive(true);

    }

    public void UnEquip(IEquipable currentMainHandEquipment)
    {
        animationListener.OnAnimationEvent -= OnAnimationEvent;
        _gameObject.SetActive(false);

    }

    private void OnAnimationEvent(string param)
    {
        switch(param)
        {
            case "Fire":
                //AttackStart();
                FireBullet();
                break;

            case "AttackEnd":
                AttackEnd();
                break;
        }
    }
    
    private void ProjectileSpawned(Transform projectile, ProjectileType projectileType)
    {
        IgnoreCollisionsWith(projectile);   //Extra
        OnProjectileSpawned(projectile, projectileType);
        //Debug.Log("Projectile Spawned! ");
    }


    private void IgnoreCollisionsWith(Transform other)  //Extra
	{
		Collider[] projectileColliders = other.GetComponentsInChildren<Collider>();

		foreach (Collider collider in colliders)
		{
			foreach (Collider projectileCollider in projectileColliders)
			{
				Physics.IgnoreCollision(collider, projectileCollider);
			}
		}
	}


    public void ChangeAmmo(IGunAmmo gunAmmo)
    {
        //Set Local Variable
        this.ammo = gunAmmo;
    }

    public bool CanUse()    
    {
        //Debug.Log("Shoot!");
        //Have we left enough time left between Shots?
        //Do we have ammo?
        //return (ammo != null && coolDown <= 0f);    // "="(Equals to) Sign is VERY IMP //

        return ammo != null && attackActive == false;   //S2 - Assignment 02
    }


    public bool CanEquipAsMain() ////////////
    {
        return true;
    }

    public bool CanEquipAsSecondary(IEquipableMain requiredEquipment) ////////
    {
        return false;
    }

    private void CollidedWithTarget(Collider collider, Vector3 collisionPoint)
    {
        //Debug.Log("Collided with Target.");
    }

    private void CollidedWithEnvironment(Vector3 collisionPoint)
    {
        Debug.Log("Collided with Environment." + collisionPoint);
    }


    //S2 - Assignment 02
    private abstract class Target
    {
        public abstract Vector3 position { get;}
    }

    private class VectorTarget : Target
    {
        private Vector3 value;

        public VectorTarget(Vector3 value)
        {
            this.value = value;
        }

        public override Vector3 position { get { return value; } } //=> value; //Short FOrm for: "{ get{ return value; }}".

    }

    private class TransformTarget : Target
    {
        private Transform value;

        public TransformTarget(Transform value)
        {
            this.value = value;
        }
        public override Vector3 position => value.position; //Short FOrm for: "{ get{ return value.position; }}".
    }
}
