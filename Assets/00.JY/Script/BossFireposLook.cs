using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireposLook : MonoBehaviour
{
    public BossMove Boss;
    Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }


    void Update()
    {
        if (Boss.playerTr != null)
            tr.LookAt(Boss.playerTr.position + (Boss.playerTr.up * 1.5f));
    }
}
