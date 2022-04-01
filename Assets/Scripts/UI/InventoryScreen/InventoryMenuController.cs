using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenuController 
{
    public Action OnClose = delegate { };
    private Transform hudTransfrom;
    private InventoryController inventoryController;
    private PlayerEquipmentController equipmentController;
    private InventorySubMenuController inventorySubMenuController;
    private CanvasGroup canvasGroup;
    private InventoryMenu menu;     //Main inevntoryUI Prefab
    private List<InventoryItem> inventoryItems; //Each Small Boxes (Rows & Clomuns)
    private List<InventoryItem> itemsInUse;


    public InventoryMenuController(Transform hudTransfrom, InventoryController inventoryController, 
    GameObject inventoryUI, PlayerEquipmentController equipmentController)
    {
        this.hudTransfrom = hudTransfrom;
        this.inventoryController = inventoryController;
        this.equipmentController = equipmentController;

        //gameObject = Inventory Inertface or UI.
        GameObject gameObject = GameObject.Instantiate(inventoryUI, hudTransfrom);

        menu = gameObject.GetComponent<InventoryMenu>();    //Main Parent Object.
        canvasGroup = gameObject.GetComponent<CanvasGroup>();   //CanvasGroup of InventoryMenu.

        inventoryItems = new List<InventoryItem>(gameObject.GetComponentsInChildren<InventoryItem>(true));  //Use "s" in GetComponents bcoz its a List.
        //We do not use GetComponents here bcoz, thses are simply the items that are being used. 
        //Bascially 2 items can be used, out of 'n' number of Items in the Inventory.
        itemsInUse = new List<InventoryItem>(); 

        //The "X" icon on the Upper-Right Corner.
        menu.closeButton.onClick.AddListener( () => Hide()); 

        inventorySubMenuController = new InventorySubMenuController(menu.subMenu, menu.subMenuButton);

        Refresh();

    }

    public void Hide()
    {
        //We use the "alpha" valuse to Hide-Unhide instead of SetActive(True & False)
        //bcoz it gives Blend in Animation by default (from 0.0 to 1.0 values).
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        OnClose();
    }
    
    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        Refresh();
    }

    //This function is used to Refresh the Inventory, to check wether there is any Update in the Inventory.
    //Like if any item(s) is used/consumed (deleted) or some new items(s) added.
    public void Refresh()   
    {
        Clear();    //Simply clears items in the Inevntory.
        
        UpdateItems();
        UpdateEquipment();        
        
    }


    private void UpdateItems()
    {
        //Here we check whats in the Inventory. 
        //Content: conatins all the items in the Inventory. It is from here we will be accessing the items.
        //Contents: Actual item representation (like DamageAmmo, Missiles, Gun, Sword, etc...).
        //InventoryItems: Graphical representation of these items in the Inventory UI Menu (each item on seperate slots).
        foreach(KeyValuePair<ItemType, ItemData> item in inventoryController.Contents)
        {
            InventoryItem slot = GetNextAvailableSlot();

            slot.gameObject.SetActive(true);
            slot.icon.gameObject.SetActive(true);
            slot.count.text = item.Value.count.ToString();    //Bcoz item.Value is an int which is converted to a String.
            slot.count.gameObject.SetActive(true);
            //slot.subMenuIcon.gameObject.SetActive(true);
            slot.icon.sprite = menu.icons.GetIcons(item.Key);   //Key: "ItemType" from KeyValuePair.

            //Create Button (Equip & Drop)
            slot.button.onClick.AddListener(() =>
            {
                List<InventorySubMenuController.Entry> entries = new List<InventorySubMenuController.Entry>();

                //Equip
                if(equipmentController.CanEquip(item.Key))  //Key: ItemType
                {
                    entries.Add(new InventorySubMenuController.Entry("Equip", () => 
                    {
                        equipmentController.Equip(item.Key);
                        Debug.Log(item.Key);
                        Refresh();
                    }));
                }

                //Drop
                if(item.Value.canDrop)
                {
                    entries.Add(new InventorySubMenuController.Entry("Drop", () => 
                    {
                        inventoryController.RemoveAll(item.Key);    //Key: ItemType 
                        //TODO  Implement a nice drop here
                        Refresh();
                    }));
                }

                inventorySubMenuController.Show(slot.menuContainer, entries);
            });
        }
    }

    //This function simply disables all the items in the Inventory.
    private void Clear()
    {
        foreach(InventoryItem item in inventoryItems)
        {
            item.gameObject.SetActive(false);
            item.icon.gameObject.SetActive(false);
            item.count.gameObject.SetActive(false);
            //item.subMenuIcon.gameObject.SetActive(false);
            //Debug.Log("Enetred Clear!");
        }

        itemsInUse.Clear();    
    }

    //Below function sets the Primary & Secondary slots with the Weapons & Ammos Icons.
    private void UpdateEquipment()
    {
        if(equipmentController.PrimaryEqiupment == null)
        {
            menu.mainEquipmentIcon.gameObject.SetActive(false);
        }
        else
        {
            menu.mainEquipmentIcon.sprite = menu.icons.GetIcons(equipmentController.PrimaryEqiupment.Type);
            menu.mainEquipmentIcon.gameObject.SetActive(true);
        }

        if(equipmentController.SecondaryEquipment == null)
        {
            menu.secondaryEquipmentIcon.gameObject.SetActive(false);
            menu.secondaryCount.gameObject.SetActive(false);
        }
        else
        {
            //Enables
            menu.secondaryEquipmentIcon.sprite = menu.icons.GetIcons(equipmentController.SecondaryEquipment.Type);
            menu.secondaryEquipmentIcon.gameObject.SetActive(true);

            //Gets Ammo Count (amount)
            menu.secondaryCount.text = inventoryController.GetCount(equipmentController.SecondaryEquipment.Type).ToString();
            menu.secondaryCount.gameObject.SetActive(true);
        }
    }
    
    private InventoryItem GetNextAvailableSlot()
    {
        //Here we visually assign each item in seperate next slots.
        foreach(InventoryItem nextSlotItem in inventoryItems)
        {
            if(!itemsInUse.Contains(nextSlotItem))
            {
                itemsInUse.Add(nextSlotItem);
                return nextSlotItem;
            }
            //Debug.Log("Enetred GetNextAvailableSlot!");
        }

        return null;
    }
}
