using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //public float healthpoint = 100f;
    public bool isdie = false;
    Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        if (isdie)
            return;

        DataManager.instance.gameData.hp -= damage;

        if (ani != null)
            ani.SetTrigger("IsHit");

        if (DataManager.instance.gameData.hp <= 0)
        {
            //Debug.Log("die");
            if (ani != null)
                ani.SetTrigger("IsDie");
            isdie = true;
            UIManager.instance.GameOverPanelOn();
        }
    }
}
