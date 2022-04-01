using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIcons : MonoBehaviour
{
    public List<ItemIcon> icons;

    public Sprite GetIcons(ItemType type)
    {
        foreach (ItemIcon icon in icons)
        {
            if (icon.type == type)
            {
                return icon.icon;
            }
        }
        return null;
    }
    public Sprite GetDesaturatedIcons(ItemType type)
    {
        foreach (ItemIcon icon in icons)
        {
            if (icon.type == type)
            {
                return icon.iconDesat;
            }
        }
        return null;
    }

}
[System.Serializable] // Add This Line To make Visible in the Hierarchy Prefab //
public class ItemIcon
{
    public ItemType type;
    public Sprite icon;
    public Sprite iconDesat;    //Desturation: turns the icon grey after it is used.

}
