using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAmmo : IGunAmmo
{
	public ItemType Type { get { return ItemType.GuardAmmo; } }
	
    public ProjectileType projectileType => ProjectileType.Guard;
	
	public bool CanEquipAsMain(IEquipableMain currentMainEquipment)
	{
		return false;
	}

	public bool CanEquipAsSecondary(IEquipableMain currentMainEquipment, IEquipable currentSecondaryEquipment)
	{
		return false;
	}

	public void Equip(IEquipable currentMainEquipment, Transform equipmentHolder){ }

	public void Destroy() { }

	public void Unequip(IEquipable currentMainEquipment){ }

    public bool CanEquipAsMain()
    {
        throw new NotImplementedException();
    }

    public bool CanEquipAsSecondary(IEquipableMain requiredEquipment)
    {
        throw new NotImplementedException();
    }

    public void UnEquip(IEquipable currentMainHandEquipment)
    {
        throw new NotImplementedException();
    }

}
