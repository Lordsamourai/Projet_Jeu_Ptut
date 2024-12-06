using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class InventoryItem : MonoBehaviour,IDropHandler
{
    public Item item;
    public RectTransform Position;
    public bool candragItem=true;
    private RectTransform RectParent;

    private void Start()
    {
        Position = GetComponent<RectTransform>();
        Position.sizeDelta = new Vector2(125, 125);
        RectParent = transform.parent.GetComponent<RectTransform>();
    }
    public void AddItemtoSlot(Item iteme)
    {
        item = iteme;
    }
    virtual public void OnDrop(PointerEventData eventData)
    {
        if (candragItem)
        {
            DropItem(eventData);
        }
    }
    public void AddtwoItem(Item item1, Item item2)
    {
        int numberofiteminexcess = 0;
        item1.amount += item2.amount;
        if (item1.amount > item1.amountStockableMax)
        {
            numberofiteminexcess = item1.amount - item1.amountStockableMax;
            item1.amount -= numberofiteminexcess;
            item1.UpdateTextAmount();
        }
        if (numberofiteminexcess == 0)
        {
            Destroy(item2.gameObject);
        }
        else
        {
            item2.amount = numberofiteminexcess;
            if (item.myText != null)
            {
                item2.UpdateTextAmount();
            }
        }
        if (item2.amount == 0)
        {
            Destroy(item2.gameObject);
        }
        
    }
    public void releaseItem()
    {
        item = null;
    }
    public void SwapTwoItem(Item item1, Item item2)
    {
        Transform parent = item2.transform.parent;
        item2.transform.SetParent(item1.transform.parent);
        item1.transform.SetParent(parent);
        item1.parent = parent.gameObject;
        parent.gameObject.GetComponent<InventoryItem>().item = item1;
        item1.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
    public void DropItem(PointerEventData eventData)
    {
        if (item == null)
        {
            eventData.pointerDrag.gameObject.transform.SetParent(Position);
            AddItemtoSlot(eventData.pointerDrag.GetComponent<Item>());
            item.parent = gameObject;
        }
        else
        {

            if (item.id == eventData.pointerDrag.GetComponent<Item>().id)
            {
                AddtwoItem(item, eventData.pointerDrag.GetComponent<Item>());
            }
            else
            {
                SwapTwoItem(item, eventData.pointerDrag.GetComponent<Item>());
            }
        }
    }
}
