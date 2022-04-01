using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySubMenuController 
{
    private GameObject subeMenuPrefab;
    private GameObject subMenuItem;
    private GameObject subMenu;

    public InventorySubMenuController(GameObject subMenuPrefab, GameObject subMenuItem)
    {
        this.subeMenuPrefab = subMenuPrefab;
        this.subMenuItem = subMenuItem;
    }

    //Below function, we Destory & Create(Instantiate) a "subMenu" GameObject for the Equip Button Option.
    public void Show(Transform parent, List<Entry> entries)
    {
        if(subMenu != null)
        {
            GameObject.Destroy(subMenu);
        }

        subMenu = GameObject.Instantiate(subeMenuPrefab, parent);

        foreach(Entry entry in entries)
        {
            GameObject buttonObj = GameObject.Instantiate(subMenuItem, subMenu.transform);
            InventorySubMenuItem item = buttonObj.GetComponent<InventorySubMenuItem>();

            item.label.text = entry.label;

            item.button.onClick.AddListener(() =>
            {
                Hide();
                entry.onClicked();
            });

        }
    }

    // UThen we Destroy it here, bcoz when clicked "Equiped" it will go to Primary of Secondary,
    //and it should not generate button again, after its use.
    public void Hide()
    {
        GameObject.Destroy(subMenu);
    }

    //-----New Class----//
    public class Entry
    {
        public string label;
        public Action onClicked;

        public Entry(string label, Action onClick)
        {
            this.label = label;
            this.onClicked = onClick;
        }
    }

}
