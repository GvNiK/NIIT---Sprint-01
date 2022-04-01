using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supplies : IEquipable
{
    public ItemType Type => throw new System.NotImplementedException();

    public bool CanEquipAsMain()
    {
        return false;
    }

    public bool CanEquipAsSecondary(IEquipableMain requiredEquipment)
    {
        return false;
    }

    public void Destroy()
    {
       
    }

    public void Equip(IEquipable currentMainHandEquipment, Transform equipmentHolder)
    {
        Debug.Log("You collected Supplies.");
    }

    public void UnEquip(IEquipable currentMainHandEquipment)
    {
        
    }
}