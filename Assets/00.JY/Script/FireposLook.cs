using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireposLook : MonoBehaviour
{
    public EnemyFindPlayer enemy;
    Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    
    void Update()
    {
        if (enemy.playerTr != null)
            tr.LookAt(enemy.playerTr.position + (enemy.playerTr.up * 1.5f));
    }
}
