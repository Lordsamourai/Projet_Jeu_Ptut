using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSpot : MonoBehaviour ,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject itemdrag = eventData.pointerDrag.GetComponent<Item>().gameObject;
        GameObject ObjectDroped = Instantiate(transform.parent.transform.parent.transform.parent.GetComponent<ListeItems>().listeallItems[itemdrag.GetComponent<Item>().id].Objet3d, new Vector3(0, 10, 0), Quaternion.identity);
        ObjectDroped.GetComponent<Item3d>().IconItem = itemdrag;
        itemdrag.GetComponent<Item>().parent.GetComponent<InventoryItem>().item = null;
        itemdrag.GetComponent<Item>().parent = null;
        GameObject canvas = new()
        {
            name = "canvasImage"
        };
        canvas.transform.parent = ObjectDroped.transform;
        canvas.AddComponent<Canvas>();
        ObjectDroped.AddComponent<MeshCollider>();
        ObjectDroped.GetComponent<MeshCollider>().convex = true;
        ObjectDroped.AddComponent<Rigidbody>();
        itemdrag.transform.SetParent(canvas.transform);
        canvas.SetActive(false);
    }
}
