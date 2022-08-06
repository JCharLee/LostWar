using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;

public class DropItem : MonoBehaviour
{
    public Item[] items = new Item[3];

    public virtual void InitItem()
    {
        items = ItemList.instance.GetRandom();
        if (items == null)
        {
            UIManager.instance.dropPanel.SetActive(false);
            Destroy(gameObject);
        }
    }
}