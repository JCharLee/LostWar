using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTest : MonoBehaviour
{
    private Transform playerTr;

    private void Awake()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ItemDrop()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/DropItem"), playerTr.position, Quaternion.identity);
    }
}
