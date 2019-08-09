using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IEventSystemHandler
{
    public bool HasItem;
    public Item CurrentItem { get; private set; }
    public Image SlotImage;
    public Image ItemImage;

    private Color deselectedColor;

    private void Awake()
    {
        deselectedColor = gameObject.GetComponent<Image>().color;
    }

    //Handle this item being selected.
    public void Select()
    {
        SlotImage.color = Color.red;
    }

    //Handle this item being unselected.
    public void Deselect()
    {
        SlotImage.color = deselectedColor;
    }

    //Displays the sprite of the item in this slot, and considers the slot as having that item.
    public void SetItem(Item item)
    {
        HasItem = true;
        CurrentItem = item;
        ItemImage.sprite = item.itemSprite;
        ItemImage.enabled = true;
    }

    //Remove the item in this slot, and considers the slot as having no item.
    public void RemoveItem()
    {
        HasItem = false;
        ItemImage.enabled = false;
    }
}
