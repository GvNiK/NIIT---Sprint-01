using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentController   
{
    //The below 2 Capital variables are used to access only in other script.
    //They are not used in this script. They return the Primary & Secondary Weapons
    public IEquipableMain PrimaryEqiupment { get{ return main; } }
    public IEquipable SecondaryEquipment { get{ return secondary; } }

    private InventoryController inventory;
    private PlayerObjectData playerObjectData;
    private Transform equipmentHolder;  
    private IEquipableMain main;    
    private IEquipable secondary;   
    private ProjectilePool projectilePool;
    private EquipmentFactory equipmentFactory;  //Creates ItemType (Melee, Sword, etc..)
    private PlayerInputBroadcaster inputBroadcaster;    //S2 - Assignment 02
    public Action OnMainUnequiped = delegate { };
    public Action OnSecondaryUnequiped = delegate { };
    public Action OnNewItemEquiped = delegate { };
    public Action<ItemType> OnEquipmentUsed = delegate { };     //Extra GuardDamage
    public Action<Guards, ItemType, Vector3> OnCollidedWithGuard = delegate { }; //Extra GuardDamage
    public Action<ItemType, Vector3> OnCollidedwithEnvironment = delegate { };  //Extra
    public Action<Transform, ProjectileType> OnProjectileSpawned = delegate { };    //Extra
    public Action<ItemType> OnItemConsumed = delegate { };
    private Collider[] playerColliders;

    public PlayerEquipmentController(   //Created in LevelController
        Transform playerTransform, InventoryController inventoryController, PlayerObjectData playerObjectData, 
        ProjectilePool projectilePool, PlayerInputBroadcaster inputBroadcaster,
        GunTargetLocator gunTargetLocator, PlayerEvents playerEvents)   //S2 - Assignment 02
    {
        this.inventory = inventoryController;
        this.playerObjectData = playerObjectData;
        this.projectilePool = projectilePool;
        this.inputBroadcaster = inputBroadcaster;
  
        this.equipmentFactory = new EquipmentFactory
        (
            playerObjectData, projectilePool, 
            playerObjectData.Sword.gameObject, 
            playerObjectData.Blaster.gameObject,

            //S2 - Assignment 02
            inputBroadcaster, gunTargetLocator, playerEvents   
        );

        inventory.OnItemCountUpdated += (item, count) =>
        {
            if(secondary != null && item == secondary.Type && count <= 0)
            {
                UnEquipSecondary();
            }
        };
        
        //Extra
        playerColliders = playerObjectData.transform.GetComponentsInChildren<Collider>();

    }

    public void OnPlayerPickedUp(Pickup pickup)
    {
        inventory.Add(pickup.itemType, pickup.quantity);    //We access the 2nd Add function which takes 2 arguments.

        if(pickup.requiredForLevelCompletion)
        {
            inventory.SetAsUndroppable(pickup.itemType);
        }
        pickup.gameObject.SetActive(false);

        EquipItemIfEmptyHanded(pickup.itemType);
    }

    private void EquipItemIfEmptyHanded(ItemType itemType)
    {
        IEquipable equipablePickup = equipmentFactory.Create(itemType);
        
        // EquipmentFactory has replaced the below code. //
        /*if(itemType == ItemType.Melee)
        {
            equipablePickup = new Sword(playerObjectData.Sword.gameObject, playerObjectData);  //Here we assign the equipablePickup(Sword) a new Sword script.
        } 
        else if(itemType == ItemType.Gun)
        {
            equipablePickup = new Gun(playerObjectData.Blaster.gameObject, playerObjectData);  //Here we assign the equipablePickup(Gun) a new Gun script.   
        }
        else if(itemType == ItemType.DamageAmmo)
        {

        }
        else
        {
            return;
        }*/

        bool isEquipable = equipablePickup != null; //Same way of writing if(!= null) then true, else(== null) then false.

        if(!isEquipable)    //If it returns a Null Value, then execute below code.
        {
            Debug.LogError("Tried to Equip Object type of : " + itemType + " wich is invalid!");
            return;
        }

        if( (main == null && equipablePickup.CanEquipAsMain()) ||
            (secondary == null && equipablePickup.CanEquipAsSecondary(main)) )
            {
                Equip(equipablePickup);
            }
            else
            {
                //To DO Cleanup bcoz we'll have constructed a class.
                equipablePickup.Destroy();
            }
    }

    public bool CanEquip(ItemType type)      //for Equip Button
    {
        IEquipable equipable = equipmentFactory.Create(type);
        bool isEquipable = equipable != null;   //True if Not Equals to Null.

        if(isEquipable == false)
        {
            return false;
        }
        
        //Here we check wether it can be equiped as Primary or Secondary.
        //If it can be equipped as any one of those, then return True, else return False.
        isEquipable = equipable.CanEquipAsMain() ||
                      equipable.CanEquipAsSecondary(main);

        return isEquipable; //return the above bool.
    }

    //This function is used to get the (IEquipable) equipable for the Equip Button - (if statement)
    //It is also an "Overload" (with same function name, but different parameters)
    public void Equip(ItemType type)
    {
        IEquipable equipable = equipmentFactory.Create(type);
        bool isEquipable = equipable != null;   //True if Not Equals to Null.

        if(isEquipable) //if true,
        {
            Equip(equipable);
        }
        else
        {
            Debug.LogError("Tried to Equip Non-Equipable item!");
        }
    }

    //Psuedo Code. Implemted for  Swtiching Weapon Logic
    /*public IEquipable SwitchWeapon(ItemType type)
    {
        IEquipable weaponToSwitch = equipmentFactory.Create(type);   //B
        bool isEquipable = weaponToSwitch != null; 

        IEquipable replaceWith = null;  //A
        IEquipable emptyWeapon = null;  //C

        if(weaponToSwitch.Type == ItemType.Gun)
        {
            //previousWeapon = currentWeapon;

            replaceWith = new Sword(equipmentFactory.sword, playerObjectData);
            //Debug.Log(type);
        }
        else
        {
            replaceWith = new Gun(equipmentFactory.gun, playerObjectData, projectilePool);
        }

        if(isEquipable)
        {
            //A = C
            //A = B
            //C = B

            //C = A
            emptyWeapon = replaceWith;
            replaceWith = null;
            //Debug.Log("C = A");

            //B = A
            replaceWith = weaponToSwitch;
            weaponToSwitch = null;
            //Debug.Log("B = A");

            //B = C
            weaponToSwitch = emptyWeapon;
        }
            Debug.Log("Weapon To Switch: " + weaponToSwitch);
            Debug.Log("Empty Weapon: " + emptyWeapon);
            Debug.Log("Replace Weapon: " + replaceWith);

        return weaponToSwitch;

    }*/

    private void Equip(IEquipable equipablePickup)
    {
        if(equipablePickup.CanEquipAsMain())
        {
            //We create these 2 Fucntions bcoz when the Player equips any Pickup Item as Main Item,
            //we want to UnEquip all the Primary & Secondary items that are currently present in Player's Hand.
            //Also if the Player is already holding a Gun and suddenly picks up Sword or assigns Sword to Primary Slot,
            //then the Gun should go back to the Inventory i.e., UnEquip along with its Bulltes(Secondary)
            UnEquipSecondary();
            UnEquipMain();

            //Casting: One Class from Another, to basically use its variables & functions
            //Here "equipablePickup" belong to "IEquipable"  Class, and we want to access the "IEquipableMain" Class to.
            //So we Cast the "equipablePickup" as "IEquipableMain" class.
            main = equipablePickup as IEquipableMain;

            main.OnProjectileSpawned += (projectile, type) =>   //Extra - Matt
            {
                IgnoreCollisionsWith(projectile);
                OnProjectileSpawned(projectile, type);
            };

            main.OnItemConsumed += (item) =>
            {
                inventory.Remove(item);
                OnItemConsumed(item);
            };

            main.OnCollidedWithGuard += (guard, type, obj) =>   //Extra
            {
                OnCollidedWithGuard(guard, type, obj);
            };

            main.OnCollidedwithEnvironment += (type, obj) => OnCollidedwithEnvironment(type, obj);

            main.OnUsed += () => OnEquipmentUsed(main.Type);    //Extra EnemyDamage

          

            inventory.EquipPrimary(equipablePickup.Type);   //Boolean: from IEquipable. It is set as ItemType.Melee(Sword) in Sword Script.
            equipablePickup.Equip(main, playerObjectData.Sword);    //Function: from IEquipable
            OnNewItemEquiped();            
        }

        else if(equipablePickup.CanEquipAsSecondary(main))
        {
            UnEquipSecondary();
            secondary = equipablePickup;
            inventory.EquipSecondary(equipablePickup.Type);
            equipablePickup.Equip(main, playerObjectData.Sword);
            OnNewItemEquiped();
        }
        else
        {
            equipablePickup.Destroy();
        }
    }

    private void IgnoreCollisionsWith(Transform other)  //Extra
    {
        Collider[] otherColliders = other.GetComponentsInChildren<Collider>();

        foreach(Collider playerCollider in playerColliders)
        {
            foreach(Collider otherCollider in otherColliders)
            {
                Physics.IgnoreCollision(playerCollider, otherCollider);
            }
        }
    }
    
    private void UnEquipSecondary()
    {
        if(secondary != null)
        {
            inventory.UnEquipSecondary();
            secondary.UnEquip(main);
            secondary.Destroy();
            secondary = null;
            OnSecondaryUnequiped();
        }
    }

    private void UnEquipMain()
    {
        if(main != null)
        {
            //These below lines helped. It UnEquips the Main Current Weapon.
            inventory.UnEquipPrimary(); 
            main.UnEquip(main);
            main.Destroy();

            main = null;
            OnMainUnequiped();
        }
    }
    
    internal void StartUse()
    {
        if(main != null && main.CanUse())
        {
            main.StartUse();
        }
    }

    public void Update() 
    {
        if(main != null)    
        {
            main.Update();
        }
    }
}
