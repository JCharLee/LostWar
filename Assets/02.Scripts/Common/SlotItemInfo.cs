using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ItemSpace;

public class SlotItemInfo : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    private Potion potion;
    public Text cnt;
    public bool isEquip = false;
    public static SlotItemInfo instance;
    [SerializeField] private GameDataObject gameDataObject;
    private float clickTime = 0;

    private void Awake()
    {
        cnt = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (item.itemType == ItemType.potion)
        {
            if (UIManager.instance.inventoryOn)
            {
                potion = item as Potion;
                cnt.text = potion.count.ToString();
                if (potion.count <= 0)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }

    public void ClickItem()
    {
        instance = this;
        UIManager.instance.ShowItemInfo(item);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - clickTime) < 0.25f)
        {
            if (!isEquip)
                GameManager.instance.OnDoubleClickEquip();
            else
                GameManager.instance.OnDoubleClickEquipOff();

            if (item.itemType == ItemType.potion)
            {
                GameManager.instance.UsePotion(item as Potion);
            }
        }
        else
            clickTime = Time.time;
    }
}
