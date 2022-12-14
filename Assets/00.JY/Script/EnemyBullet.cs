using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody rigid;
    Transform tr;
    public float damage = 25f;
    public float shotforce = 500f;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        rigid.AddForce(tr.forward * shotforce);
    }
    void Update()
    {
        Destroy(this.gameObject, 3.00f);
    }

    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<Health>();
        if (health != null)
        {
            health.takeDamage(damage);
        }
        Destroy(this.gameObject);
    }
}
