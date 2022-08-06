using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    Rigidbody rigid;
    Transform tr;
    private float damage = 10f;
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
        var health = other.GetComponent<EnemyHealth>();
        if(health != null)
        {
            health.takeDamage(damage, other.transform.localPosition);
        }
        Destroy(this.gameObject);
    }
}
