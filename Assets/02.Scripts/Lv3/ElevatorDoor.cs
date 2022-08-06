using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : Lv3_Door
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        speed = 5f;
        prompt = "[F] 문 열기";
        base.Start();
    }

    public override bool Action(PlayerInteraction interactor)
    {
        return base.Action(interactor);
    }

    public override IEnumerator DoorAction()
    {
        return base.DoorAction();
    }
}