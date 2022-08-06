using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    GameObject smallexp;
    
    void Start()
    {
        smallexp = Resources.Load("SmallExplosion") as GameObject;
    }

    
    void Update()
    {
        Destroy(this.gameObject, 1.9f);
    }
    private void OnParticleCollision(GameObject other)
    {
        var exp = Instantiate(smallexp, other.transform.position, other.transform.rotation);
        expDamage();
        Destroy(this.gameObject, 0.1f);
        Destroy(exp, 1.5f);
    }

    void expDamage()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 10f);
        foreach(Collider col in cols)
        {
            Health mob = col.GetComponent<Health>();
            if(mob != null)
            {
                mob.takeDamage(100f);
            }
        }
    }
}
