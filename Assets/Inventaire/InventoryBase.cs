using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryBase : MonoBehaviour
{
    public List<InventoryItem> ListeObjets;
    virtual public void StartInventaire()
    {
        ListeObjets = GetComponentsInChildren<InventoryItem>().ToList();
        foreach (InventoryItem item in ListeObjets)
        {
            item.gameObject.GetComponent<Image>().sprite = transform.parent.GetComponent<ListeItems>().Background;
        }
        ListeObjets.RemoveRange(ListeObjets.Count - 4, 4);
    }
    public void AddIconIventaire(int id, int amount)
    {
        if (id < transform.parent.GetComponent<ListeItems>().listeallItems.Count() && id >= 0)
        {
            int numbericons = 1 + amount / transform.parent.GetComponent<ListeItems>().listeallItems[id].Icon.GetComponent<Item>().amountStockableMax;
            if (transform.parent.GetComponent<ListeItems>().listeallItems[id].Icon.GetComponent<Item>().amountStockableMax == 1)
            {
                --numbericons;
            }
            int numberdone = 0;
            while (numberdone < numbericons)
            {
                bool asadded = false;
                GameObject icon = Instantiate(transform.parent.GetComponent<ListeItems>().listeallItems[id].Icon);
                if (numberdone == numbericons - 1)
                {
                    icon.GetComponent<Item>().amount = amount;
                }
                else
                {
                    icon.GetComponent<Item>().amount = icon.GetComponent<Item>().amountStockableMax;
                }
                for (int i = 0; i < ListeObjets.Count; i++)
                {
                    if (ListeObjets[i].item == null)
                    {
                        ListeObjets[i].AddItemtoSlot(icon.GetComponent<Item>());
                        icon.GetComponent<Item>().gameObject.transform.SetParent(ListeObjets[i].transform);
                        ListeObjets[i].item.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                        ListeObjets[i].item.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                        icon.GetComponent<Item>().parent = ListeObjets[i].gameObject;
                        ListeObjets[i].item.gameObject.GetComponent<Image>().sprite = transform.parent.GetComponent<ListeItems>().listeallItems[id].Icon.GetComponent<Item>().iconImage;
                        icon.GetComponent<Item>().CreateTextAmount();
                        asadded = true;

                        break;
                    }
                    else
                    {
                        if (ListeObjets[i].item.GetComponent<Item>().id == icon.GetComponent<Item>().id &&
                            icon.GetComponent<Item>().amountStockableMax >= ListeObjets[i].item.GetComponent<Item>().amount + amount)
                        {
                            ListeObjets[i].AddtwoItem(ListeObjets[i].item, icon.GetComponent<Item>());
                            ListeObjets[i].item.UpdateTextAmount();
                            asadded = true;
                            break;

                        }
                    }
                }
                if (!asadded)
                {
                    GameObject ObjectDroped =Instantiate(transform.parent.GetComponent<ListeItems>().listeallItems[id].Objet3d,new Vector3(0,10,0),Quaternion.identity);
                    ObjectDroped.AddComponent<MeshCollider>();
                    ObjectDroped.GetComponent<MeshCollider>().convex = true;
                    ObjectDroped.AddComponent<Rigidbody>();
                    Debug.Log("Non Ajouté");
                    Destroy(icon);
                }
                else
                {
                    amount -= icon.GetComponent<Item>().amountStockableMax;
                }
                numberdone++;
            }
        }
        else
        {
            Debug.LogError("Mauvais id");
        }
    }
    public bool AddIconIventaire(GameObject item) {

        if (item.GetComponent<Item>().amount == 0)
        {
            return false;
        }
        for (int i = 0; i < ListeObjets.Count; i++)
        {
            if (ListeObjets[i].item == null && item.GetComponent<Item>().amount<=item.GetComponent<Item>().amountStockableMax && item.GetComponent<Item>().amount>0)
            {
                ListeObjets[i].AddItemtoSlot(item.GetComponent<Item>());
                item.GetComponent<Item>().gameObject.transform.SetParent(ListeObjets[i].transform);
                ListeObjets[i].item.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                ListeObjets[i].item.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                item.GetComponent<Item>().parent = ListeObjets[i].gameObject;
                ListeObjets[i].item.gameObject.GetComponent<Image>().sprite = item.GetComponent<Item>().iconImage;
                item.GetComponent<Item>().CreateTextAmount();
                return true;
            }
            else if (ListeObjets[i].item == null && item.GetComponent<Item>().amount > item.GetComponent<Item>().amountStockableMax)
            {
                GameObject itemnew = Instantiate(item);
                if (itemnew.GetComponent<Item>().amount <= 0)
                {
                    break;
                }
                itemnew.GetComponent<Item>().amount = itemnew.GetComponent <Item>().amountStockableMax;
                item.GetComponent<Item>().amount -=item.GetComponent<Item>().amountStockableMax;
                ListeObjets[i].AddItemtoSlot(itemnew.GetComponent<Item>());
                itemnew.GetComponent<Item>().gameObject.transform.SetParent(ListeObjets[i].transform);
                ListeObjets[i].item.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                ListeObjets[i].item.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                itemnew.GetComponent<Item>().parent = ListeObjets[i].gameObject;
                ListeObjets[i].item.gameObject.GetComponent<Image>().sprite = itemnew.GetComponent<Item>().iconImage;
                itemnew.GetComponent<Item>().CreateTextAmount();
            }
            else
            {
                if (ListeObjets[i].item.GetComponent<Item>().id == item.GetComponent<Item>().id &&
                    item.GetComponent<Item>().amountStockableMax >= ListeObjets[i].item.GetComponent<Item>().amount + item.GetComponent<Item>().amount)
                {
                    ListeObjets[i].AddtwoItem(ListeObjets[i].item, item.GetComponent<Item>());
                    ListeObjets[i].item.UpdateTextAmount();

                }
            }
        }
        return false;
    }
    public bool isfull()
    {
        foreach (var item in ListeObjets)
        {
            if (item.item == null)
            {
                return false;
            }
        }
        return true;
    }
    virtual public void ToogleCanDragitem()
    {

        foreach(var item in ListeObjets)
        {
            item.candragItem = !item.candragItem;
            if(item.item != null)
            {
                item.item.candragitem = !item.item.candragitem;
            }
        }
    }
}

