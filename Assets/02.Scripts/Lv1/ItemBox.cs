using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;

public class ItemBox : DropItem, IInteraction
{
    private float castTime = 2f;

    [SerializeField] string prompt;

    private UIManager uiManager;
    private BasicBehaviour basicBehaviour;

    public string interactionPrompt => prompt;

    void Start()
    {
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        basicBehaviour = FindObjectOfType<BasicBehaviour>();

        prompt = "[F] 상자 열기";
        InitItem();
    }

    private void Update()
    {
        if (items[0] == null && items[1] == null && items[2] == null)
        {
            if (uiManager.dropOn)
                uiManager.CloseDropPanel();
            gameObject.layer = 0;
            GameObject.FindGameObjectWithTag("Player").transform.GetChild(9).gameObject.SetActive(true);
            return;
        }
    }

    public bool Action(PlayerInteraction interactor)
    {
        uiManager.moveRoutine = StartCoroutine(ItemboxOpen());
        return true;
    }

    IEnumerator ItemboxOpen()
    {
        uiManager.castRoutine = StartCoroutine(uiManager.InteractionCasting(castTime));
        if (uiManager.castRoutine == null) uiManager.moveRoutine = null;
        yield return new WaitForSeconds(castTime);
        uiManager.OpenDropPanel(this);
    }

    public override void InitItem()
    {
        items[0] = ItemList.instance.Get("Sword");
        items[1] = ItemList.instance.Get("Pistol");
        items[2] = ItemList.instance.Get("HP Potion");
    }
}