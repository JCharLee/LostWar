using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    Collider col;
    public int damage = 50;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<Health>();
        if(health != null)
        {
            health.takeDamage(damage);
            col.enabled = false;
        }
    }
}
