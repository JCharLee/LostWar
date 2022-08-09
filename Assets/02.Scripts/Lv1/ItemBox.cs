using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;

public class ItemBox : DropItem
{
    private float castTime = 2f;

    private void Start()
    {
        prompt = "[F] 상자 열기";
        InitItem();
    }

    private void Update()
    {
        if (items[0] == null && items[1] == null && items[2] == null)
        {
            if (UIManager.instance.dropOn)
                UIManager.instance.CloseDropPanel();
            gameObject.layer = 0;
            GameObject.FindGameObjectWithTag("Player").transform.GetChild(9).gameObject.SetActive(true);
            Destroy(FindObjectOfType<ItemBox>());
            return;
        }
    }

    public override bool Action(PlayerInteraction interactor)
    {
        UIManager.instance.moveRoutine = StartCoroutine(ItemboxOpen());
        return true;
    }

    IEnumerator ItemboxOpen()
    {
        UIManager.instance.castRoutine = StartCoroutine(UIManager.instance.InteractionCasting(castTime));
        if (UIManager.instance.castRoutine == null) UIManager.instance.moveRoutine = null;
        yield return new WaitForSeconds(castTime);
        UIManager.instance.OpenDropPanel(this);
    }

    public override void InitItem()
    {
        items[0] = ItemList.instance.Get("Sword");
        items[1] = ItemList.instance.Get("Pistol");
        items[2] = ItemList.instance.Get("HP Potion");
    }
}