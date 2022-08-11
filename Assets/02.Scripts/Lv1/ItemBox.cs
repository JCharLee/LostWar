using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;

public class ItemBox : DropItem
{
    private float castTime = 2f;

    private IInteraction interactable;
    private Coroutine castRoutine;
    private Coroutine moveRoutine;

    private BasicBehaviour basicBehaviour;

    public override Coroutine CastRoutine => castRoutine;
    public override Coroutine MoveRoutine => moveRoutine;
    public override float CastingTime => castTime;

    private void Start()
    {
        interactable = this;

        basicBehaviour = FindObjectOfType<BasicBehaviour>();
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

        if (interactable != PlayerInteraction.instance.interactable) return;

        if (UIManager.instance.casting)
        {
            if (basicBehaviour.IsMoving())
            {
                StopCoroutine(castRoutine);
                StopCoroutine(moveRoutine);
                UIManager.instance.StopCasting();
                castRoutine = null;

                StartCoroutine(UIManager.instance.NoticeText(false, "중간에 움직여서 취소됐습니다."));
            }
        }
    }

    public override bool Action(PlayerInteraction interactor)
    {
        moveRoutine = StartCoroutine(ItemboxOpen());
        return true;
    }

    IEnumerator ItemboxOpen()
    {
        castRoutine = StartCoroutine(UIManager.instance.InteractionCasting(castTime));
        yield return new WaitForSeconds(castTime);
        castRoutine = null;
        moveRoutine = null;
        UIManager.instance.OpenDropPanel(this);
    }

    public override void InitItem()
    {
        items[0] = ItemList.instance.Get("Sword");
        items[1] = ItemList.instance.Get("Pistol");
        items[2] = ItemList.instance.Get("HP Potion");
    }
}