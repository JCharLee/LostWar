using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondRoomDoor : Lv3_Door
{
    private string cautionText;

    private UIManager uiManager;

    protected override void Awake()
    {
        base.Awake();
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    protected override void Start()
    {
        speed = 5f;
        prompt = "[F] 문 열기";
        base.Start();
        cautionText = "첫 번째 카드키를 먼저 구해야 합니다.";
    }

    public override bool Action(PlayerInteraction interactor)
    {
        var key = interactor.GetComponent<PlayerInteraction>();

        if (key == null) return false;

        if (key.hasKey1)
        {
            return base.Action(interactor);
        }

        if (!uiManager.alert)
            StartCoroutine(uiManager.NoticeText(false, cautionText));
        return false;
    }

    public override IEnumerator DoorAction()
    {
        return base.DoorAction();
    }
}
