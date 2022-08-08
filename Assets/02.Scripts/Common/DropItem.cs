using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;

public class DropItem : MonoBehaviour, IInteraction
{
    public Item[] items = new Item[3];

    [SerializeField] protected string prompt;

    public string interactionPrompt => prompt;

    private void Start()
    {
        prompt = "[F] 아이템 줍기";
        InitItem();
    }

    private void Update()
    {
        if (items[0] == null && items[1] == null && items[2] == null)
        {
            if (UIManager.instance.dropOn)
                UIManager.instance.CloseDropPanel();
            Destroy(this.gameObject);
            return;
        }
    }

    public virtual bool Action(PlayerInteraction interactor)
    {
        UIManager.instance.OpenDropPanel(this);
        return true;
    }

    public virtual void InitItem()
    {
        items = ItemList.instance.GetRandom();
    }
}