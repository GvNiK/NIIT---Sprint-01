using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGunAmmo : IEquipable
{
    ProjectileType projectileType { get; }
}

