using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentFactory 
{
    private PlayerObjectData playerObjectData;
    private ProjectilePool projectilePool;
    private PlayerInputBroadcaster inputBroadcaster;    //S2 - Assignment 02
    public GameObject sword;
    public GameObject gun;
    private GunTargetLocator gunTargetLocator;
    private PlayerEvents playerEvents;

    public EquipmentFactory(
        PlayerObjectData playerObjectData, ProjectilePool projectilePool, 
        GameObject sword, GameObject gun, 
        PlayerInputBroadcaster inputBroadcaster, GunTargetLocator gunTargetLocator, PlayerEvents playerEvents)  //S2 - Assignment 02
    {
        this.playerObjectData = playerObjectData;
        this.projectilePool = projectilePool;
        this.sword = sword;
        this.gun = gun;

        //S2 - Assignment 02
        this.inputBroadcaster = inputBroadcaster;   
        this.gunTargetLocator = gunTargetLocator;
        this.playerEvents = playerEvents;
    }

    public IEquipable Create(ItemType type)
    {
        switch(type)
        {
            case ItemType.Melee:
                return new Sword(sword, playerObjectData);
            case ItemType.Gun:
                return new Gun(gun, playerObjectData, gunTargetLocator, playerEvents, projectilePool, inputBroadcaster);    //S2 - Assignment 02
            case ItemType.DamageAmmo:
                return new DamageAmmo(); 
            case ItemType.ExplosiveAmmo:
                return new ExplosiveAmmo(); 
            case ItemType.Supplies:
                return new Supplies();         
        }

        return null;
    }

}
