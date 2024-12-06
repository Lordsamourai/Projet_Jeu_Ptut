using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventaire : InventoryBase
{
    private List<InventoryItem> ListeArmure;

    override public void StartInventaire()
    {
        base.StartInventaire();
        ListeArmure = GetComponentsInChildren<InventoryItem>().ToList();
        foreach (InventoryItem item in ListeArmure)
        {
            item.gameObject.GetComponent<Image>().sprite = transform.parent.GetComponent<ListeItems>().Background;
        }
        ListeArmure.RemoveRange(0, ListeObjets.Count);
    }
}

