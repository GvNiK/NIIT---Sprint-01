using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAmmo : IGunAmmo
{
   public ProjectileType projectileType => ProjectileType.Explosive;

    public ItemType Type { get { return ItemType.ExplosiveAmmo; } }

    public bool CanEquipAsMain()
    {
        return false;
    }

    public bool CanEquipAsSecondary(IEquipableMain requiredEquipment)
    {
        //Return True, only if the the requiredEquipment is Gun.
        return requiredEquipment.Type == ItemType.Gun;
    }

    public void Equip(IEquipable currentMainHandEquipment, Transform equipmentHolder)
    {
        (currentMainHandEquipment as Gun).ChangeAmmo(this);
    }

    public void UnEquip(IEquipable currentMainHandEquipment)
    {
        (currentMainHandEquipment as Gun).ChangeAmmo(null);
        Debug.Log("Ammo --- UnEquipping!");

    }
    public void Destroy()
    {
        
    }
}

