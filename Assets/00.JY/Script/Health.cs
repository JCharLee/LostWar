using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float healthpoint = 100f;
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

        healthpoint = Mathf.Clamp(healthpoint - damage, 0f, healthpoint);
        if (ani != null)
            ani.SetTrigger("IsHit");

        if (healthpoint <= 0)
        {
            //Debug.Log("die");
            if (ani != null)
                ani.SetTrigger("IsDie");
            isdie = true;
        }
    }
}
