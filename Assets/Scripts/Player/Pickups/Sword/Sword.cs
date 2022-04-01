using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : IEquipableMain
{
    public Action<ItemType> OnItemConsumed { get{ return toConsume; } set { toConsume = value; }}
    public Action OnUsed { get{ return toUse; } set{ toUse = value; } }
    public Action<Guards, ItemType, Vector3> OnCollidedWithGuard { get { return toConsumeGuardCollision; } set { toConsumeGuardCollision = value; } }    //Extra GuardDamage
    public Action<ItemType, Vector3> OnCollidedwithEnvironment { get => toConsumeEnvironment; set => toConsumeEnvironment = value; }    //Extra
   
    public ItemType Type => ItemType.Melee; //Bcoz Sword is a Melee Item.   "=>" = get{ ;}   

    public Action<Transform, ProjectileType> OnProjectileSpawned { get { return ProjectileConsumed; } set => ProjectileConsumed = value; }


    private Action<Transform, ProjectileType> ProjectileConsumed= delegate { };
    private Action<ItemType, Vector3> toConsumeEnvironment = delegate { };  //Extra
    private Action<Guards, ItemType, Vector3> toConsumeGuardCollision = delegate { };    
    private Action<ItemType> toConsume;
    private Action toUse;

    private PlayerSettings playerSettings;

    private GameObject _gameObject; //Sword GameObject attached from "EquipmentFactory"
    private PlayerObjectData playerObjectData;
    private CollisionCallbacks collisionCallbacks;
    //private SwordAttackAnimation attackAnimation;
    private Animator animator;  //S2 - Assignment 02
    private AnimationListener animationListener;    //S2 - Assignment 02
    private TrailRenderer swordTrail;   //S2 - Assignment 02
    private bool chainAttack;

   

    public Sword(GameObject gameObject, PlayerObjectData playerObjectData)
    {
        this._gameObject = gameObject;
        this.playerObjectData = playerObjectData;
        this.playerSettings = playerObjectData.GetComponent<PlayerSettingsHolder>().playerSettings;
        animator = playerObjectData.PlayerAnimatior; //.GetComponent<Animator>();   //S2 - Assignment 02
        animationListener = animator.gameObject.GetComponent<AnimationListener>();  //S2 - Assignment 02
        swordTrail = gameObject.GetComponentInChildren<TrailRenderer>();    //S2 - Assignment 02

        //We add <>(true) becoz it checks hidden GameObjects too. If we do not write the (true),
        //it will not look through the hidden GameObjects
        collisionCallbacks = gameObject.GetComponentInChildren<CollisionCallbacks>(true);
        //collisionCallbacks.gameObject.SetActive(false);   //DO NOT ENABLE - Turns the Sword Collison Off in the Scene.
    }

    public bool CanEquipAsMain()
    {
        return true;
    }

    public bool CanEquipAsSecondary(IEquipableMain requiredEquipment)
    {
        return false;
    }


    public bool CanUse()    //Check Animation is Active
    {
        return animator.GetBool("Swipe") == false || chainAttack == false;
    }

    public void StartUse()  //S2 - Assignment 02
    {   
        //If the "Swipe" is True, then go inside & Return.
        if(animator.GetBool("Swipe"))   //Default: True. Also can be written as: animator.GetBool("Swipe") == true.
        {
            chainAttack = true;
            return;
        }

        //If not, then set it True here in the below line.
        animator.SetBool("Swipe", true);

        //attackAnimation = new SwordAttackAnimation(_gameObject.transform);
        //attackAnimation.OnAttackStarted += () => EnableHitBox();    //Turn On Collison
        //attackAnimation.OnAttackStarted += () => DisableHitBox();   //Turn Off Collison
        //attackAnimation.Start();
    }

    public void Equip(IEquipable currentMainHandEquipment, Transform equipmentHolder)
    {
        _gameObject.SetActive(true);
        collisionCallbacks.OnTriggerEntered += SwrodCollision;
        animationListener.OnAnimationEvent += OnAnimationEvent; //S2 - Assignment 02
    }

    public void UnEquip(IEquipable currentMainHandEquipment)
    {
        _gameObject.SetActive(false);
        collisionCallbacks.OnTriggerEntered -= SwrodCollision;
        animationListener.OnAnimationEvent -= OnAnimationEvent; //S2 - Assignment 02
    }

    private void OnAnimationEvent(string param) //S2 - Assignment 02
    {
        //The String "param" checks every String on an ANimation Clip or State.
        //And if the String matches any of the Below Names, it executes the respective Functions.
        switch(param)
        {
            case "SwipeOneStart":
            case "SwipeTwoStart":
                AttackStart();
                break;
                
            case "SwipeOneEnd":
            case "SwipeTwoEnd":
                AttackEnd();
                break;
        }
    }


    private void SwrodCollision(Collider obj)
    {
        //Debug.Log("Sword Collider with: " + obj.name);

        //ClosestPointOnBounds = The closest point to the bounding box of the attached collider.
        Vector3 closestPoint = obj.ClosestPointOnBounds(_gameObject.transform.position);
        if(obj.transform.tag.Equals("Enemy"))   //IMP: TAG was missing - Thnks Rajath!
        {
            Guards guard = obj.transform.GetComponent<Guards>();
            guard.TakeDamage(playerSettings.swordDamage, playerObjectData.transform);

            OnCollidedWithGuard(guard, Type, closestPoint);
            Debug.Log(playerSettings.swordDamage);
            //DisableHitBox();
            AttackEnd();
        }
		else
		{
			OnCollidedwithEnvironment(ItemType.Melee, closestPoint);
		}
    }

    public void AttackStart()   //S2 - Assignment 02
    {
        //Debug.Log("OnAttack - from Sword");
        swordTrail.emitting = true; //Start Trail.
        collisionCallbacks.gameObject.SetActive(true);
        OnUsed();
        //EnableHitBox();
    }

    private void AttackEnd()    //S2 - Assignment 02
    {
        swordTrail.emitting = false;    //End Trail.
        collisionCallbacks.gameObject.SetActive(false);

        //If the ChainAttack is True, that means we have 1 Animation Pending, and after finishing that we set it to False.
        if(chainAttack) //Default: True.
        {
            chainAttack = false;
        }
        //If the ChainAttack is False, that means we do not have any Animations Pending, so we set the Swipe also to False.
        else
        {
            animator.SetBool("Swipe", false);
        }

    }

    /*public void EnableHitBox()
    {
        //Debug.Log("EnableHitBox - from Sword");
        collisionCallbacks.gameObject.SetActive(true);
    }

    public void DisableHitBox()
    {
        //Debug.Log("DisableHitBox - from Sword");
        collisionCallbacks.gameObject.SetActive(false);
    }*/

    public void EndUse()
    {
        
    }

    public void Update()
    {
        /*if(attackAnimation != null)
        {
            attackAnimation.Update();
        }*/
    }

    public void Destroy()
    {
        collisionCallbacks.OnTriggerEntered -= SwrodCollision;
    }
}
