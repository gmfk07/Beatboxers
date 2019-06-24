using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IEventSystemHandler
{
    public Image SlotImage;
    public Image ItemImage;

    //Handle this item being selected
    public void Select()
    {
        SlotImage.color = Color.red;
    }

    //Handle this item being unselected
    public void Deselect()
    {
        SlotImage.color = Color.white;
    }

    //Displays the given sprite as the item in this slot.
    public void SetItemImageSprite(Sprite sprite)
    {
        ItemImage.sprite = sprite;
    }
}
