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
        var HP = GameManager.instance.gameDataObject.Hp;
    }

    
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        if (isdie)
            return;

        GameManager.instance.gameDataObject.Hp -= damage;

        if (ani != null)
            ani.SetTrigger("IsHit");

        if (GameManager.instance.gameDataObject.Hp <= 0)
        {
            //Debug.Log("die");
            if (ani != null)
                ani.SetTrigger("IsDie");
            isdie = true;
        }
    }
}
