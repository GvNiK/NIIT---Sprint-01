using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController 
{
   public const int MAX_NUMBER_PER_CATEGORY = 99;
   public Action<ItemType, int> OnItemCountUpdated = delegate { };  //This function is used to keep track of items (Type as well as its Count).

    //A Dictionary is a collection of Values that can be accessed by One or More Keys.
    //Dictionary<Key, Value>
    private Dictionary<ItemType, ItemData> heldItems;
    private ItemData primary;
    private ItemData secondary; 

    public InventoryController()
    {
        heldItems = new Dictionary<ItemType, ItemData>();
        foreach(ItemType pickupItem in Enum.GetValues(typeof(ItemType)))
        {
            heldItems[pickupItem] = new ItemData();  //We do this bcoz we need to add New ItemData values(Count & canDrop) to each ItemType(Melee, Gun, etc...)
        }
    }

    public bool CanAdd(ItemType type)
    {
        return heldItems[type].count < MAX_NUMBER_PER_CATEGORY;
    }

        //The below function simply Adds items to the Inventory and Notifies using the delegate function.
        public void Add(ItemType type)  
        {
            if(CanAdd(type))
            {
                heldItems[type].count++;
                OnItemCountUpdated(type, heldItems[type].count);
            }
        }

    //The below function helps to only pickup items based on Inventory's Capacity.
    //i.e. suppose if there are 20 ammos stack on the gorund, and the Inventory can only add 10 more ammos stack,
    //then the Player will take only 10 ammos stack into the inventory and leave the rest 10 on the ground.
    public void Add(ItemType type, int amount)  
    {
        for (int i = 0; i < amount; i++)
        {
            if(CanAdd(type))
            {
                heldItems[type].count++;
                OnItemCountUpdated(type, heldItems[type].count);
            }
        }
    }

    public bool CanRemove(ItemType type)
    {
        return heldItems[type].count > 0 && heldItems[type].canDrop;    //canDrop = true. (Already set to True in the "ItemData" Constructor)
    }
    public void Remove(ItemType type)
    {
        if(CanRemove(type))
        {
            heldItems[type].count--;
            OnItemCountUpdated(type, heldItems[type].count);
        }
    }

    public void RemoveAll(ItemType type)
    {
        heldItems[type].count = 0;
        OnItemCountUpdated(type, heldItems[type].count);
    }

    public void SetAsUndroppable(ItemType type)
    {
        heldItems[type].canDrop = false;
    }
    
    public int GetCount(ItemType type)
    {
        return heldItems[type].count;
    }

    public void EquipPrimary(ItemType type)
    {
        primary = heldItems[type];
    }
    public void UnEquipPrimary()
    {
        primary = null;
    }

    public void EquipSecondary(ItemType type)
    {
        secondary = heldItems[type];
    }

    public void UnEquipSecondary()
    {
        secondary = null;
    }    

    //The below function is used get Everything thats in our Inventory but not in our Hands.
    //To access all the Contents available in our Inventory, but are not available in the Primary or Secondary slots.
    public Dictionary<ItemType, ItemData> Contents
    {
        get     //Used to get values. In this case gets toReturn.
        {
            Dictionary<ItemType, ItemData> toReturn = new Dictionary<ItemType, ItemData>();

            //KeyValuePair = Used to access one element of the Ditionary.
            foreach(KeyValuePair<ItemType, ItemData> item in heldItems)
            {
                if(item.Value != primary && 
                    item.Value != secondary &&
                    item.Value.count > 0)
                {
                    toReturn[item.Key] = item.Value.Copy(); //We simply copy the values and store it. (i.e., create a Clone or Duplicate)
                }
            }

            return toReturn;
        }
    }


}
 
public enum ItemType
{
    Melee,
    Gun,
    DamageAmmo,
    ExplosiveAmmo,
    GuardAmmo,
    Supplies
}

public class ItemData   //Class
{
    public int count;
    public bool canDrop;

    public ItemData()   //Constructor
    {
        count = 0;
        canDrop = true;
    }
    public ItemData Copy()  //Shallow Copy
    {
        return new ItemData()
        {
            count = count,
            canDrop = canDrop
        };
    }
}
