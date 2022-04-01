using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipableMain : IEquipable
{
    bool CanUse();
    void StartUse();
    void EndUse();
    void Update();

    Action<ItemType, Vector3> OnCollidedwithEnvironment { get; set; }   //Extra
    Action<Guards, ItemType, Vector3> OnCollidedWithGuard { get; set; }
    Action<Transform, ProjectileType> OnProjectileSpawned { get; set; }
    Action<ItemType> OnItemConsumed { get; set; }
    Action OnUsed { get; set; }

}
