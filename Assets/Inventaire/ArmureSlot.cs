using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ArmureSlot : InventoryItem
{
    public Item.TypeItem ItemNeeded;

    override public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.GetComponent<Item>().TypedelItem == ItemNeeded)
        {
            DropItem(eventData);
        }
    }

}
