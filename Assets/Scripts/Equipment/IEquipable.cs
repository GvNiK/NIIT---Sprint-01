using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable 
{
   bool CanEquipAsMain();

   //We pass in a Parameter bcoz we need the Ammo as a necessity for the Gun Equipment.
   bool CanEquipAsSecondary(IEquipableMain requiredEquipment); 

   void Equip(IEquipable currentMainHandEquipment, Transform equipmentHolder);

   void UnEquip(IEquipable currentMainHandEquipment);
   void Destroy();

   ItemType Type { get; }

}
 