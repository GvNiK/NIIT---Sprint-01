using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Action OnInventoryRequested = delegate { };
	public CanvasGroup gameplayHUDCanvasGroup;
    
    //S2 - Assignment 02
    public JoystickController joystickController;   
    public CanvasGroup equipmentGroup;
    public Image equipmentIcon;
    public Image equipmentBorder;
    public CanvasGroup secondaryGroup;
    public Image secondaryIcons;
    public TextMeshProUGUI secondaryText;
    public ItemIcons icons;
    private InventoryController inventoryController;    


    public Image playerHealthBar;

	public Sprite interactIcon;

	private Player player;
	private float playerMaxHP = 100f;

	public void SetupHUDController(Player player, InventoryController inventoryController, PickupEvents pickupEvents)  //S2 - Assignment 02
    {
		this.player = player;   
        this.inventoryController = inventoryController;     //S2 - Assignment 02

        //S2 - Assignment 02
        joystickController.Setup(player.Broadcaster.Callbacks); 

        player.playerEquipment.OnNewItemEquiped += UpdateSecondaryEquipment;
        player.playerEquipment.OnItemConsumed += (ItemType) => UpdateSecondaryEquipment();  //Bcoz it is an "Action<ItemType>" delegate which takes a Parameter.

        pickupEvents.OnPickupEventCollected += (pickupEventsp) =>   //S2 - Assignment 02
        {
            UpdateSecondaryEquipment();
        };
        
        UpdateSecondaryEquipment(); //COnstant Update - id the above 2 conditions doesn't meet, then this Func. will execute.
    }

	public void ShowHUD()
    {
        gameplayHUDCanvasGroup.alpha = 1f;
    }

    public void HideHUD()
    {
        gameplayHUDCanvasGroup.alpha = 0f;
    }

    

    public void ShowInevntory()
    {
        OnInventoryRequested();
    }

    public void UpdatePlayerHealth(float newValue)
    {
        playerHealthBar.fillAmount = newValue / playerMaxHP;
    }

    public void Update()
    {
        UpdateEquipmentEnabledState();
    }

    private void UpdateEquipmentEnabledState()
    {
        if(player.Interaction.CanInteract())    //Hand Icon
        {
            equipmentIcon.sprite = interactIcon;
            equipmentBorder.enabled = true;
            equipmentGroup.alpha = 1;
            secondaryGroup.alpha = 0;
        }
        else
        {
            if(player.playerEquipment.PrimaryEqiupment == null)
            {
                equipmentGroup.alpha = 0;
            }
            else    //Primary or Main Icon - Active & In-Active.
            {
                if(player.playerEquipment.PrimaryEqiupment.CanUse())
                {
                    equipmentIcon.sprite = icons.GetIcons(player.playerEquipment.PrimaryEqiupment.Type);
                    equipmentBorder.enabled = true;
                }
                else
                {
                    equipmentIcon.sprite = icons.GetDesaturatedIcons(player.playerEquipment.PrimaryEqiupment.Type);
                    equipmentBorder.enabled = false;
                }

                equipmentGroup.alpha = 1;
            }
        }

        if(player.playerEquipment.SecondaryEquipment == null)
        {
            secondaryGroup.alpha = 0;
        }
        else
        {
            secondaryGroup.alpha = 1;
        }
    }
    
    private void UpdateSecondaryEquipment()
    {
        if(player.playerEquipment.SecondaryEquipment != null)
        {
            secondaryIcons.sprite = icons.GetIcons(player.playerEquipment.SecondaryEquipment.Type);
            secondaryText.text = inventoryController.GetCount(player.playerEquipment.SecondaryEquipment.Type).ToString();
        }
    }
}
