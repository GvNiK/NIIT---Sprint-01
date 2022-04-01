using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryMenu : MonoBehaviour
{
    public RectTransform itemGrid;
    public Button closeButton;
    public Image mainEquipmentIcon;
    public Image secondaryEquipmentIcon;
    public TextMeshProUGUI secondaryCount;
    public GameObject subMenu;
    public GameObject subMenuButton;
    public ItemIcons icons;
}
