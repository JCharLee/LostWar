using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : MonoBehaviour
{
    GameObject bulletPrefab;
    public Transform firePos;
    ParticleSystem muzzleflash;

    void Start()
    {
        bulletPrefab = Resources.Load<GameObject>("Bullet");
        firePos = transform.GetChild(3).GetComponent<Transform>();
        muzzleflash = firePos.GetComponentInChildren<ParticleSystem>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }

    public void Fire()
    {
        GameObject _bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        muzzleflash.Play();
    }
}
