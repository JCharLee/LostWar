using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    Rigidbody rigid;
    Animator ani;
    GameObject bulletPrefab;
    BossMove bossMove;
    EnemyHealth EHealth;
    Transform tr;
    ParticleSystem muzzleflash;
    public Transform firepos;
    float nextFire = 0;
    readonly float FireRate = 0.3f;
    public bool isCombat = false;
    int remainAmmo = 3;
    readonly int rechargeAmmo = 3;
    public bool isReload = false;
    public int pattern;
    public bool movepattern = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        bulletPrefab = Resources.Load<GameObject>("EBullet");
        tr = GetComponent<Transform>();
        bossMove = GetComponent<BossMove>();
        EHealth = GetComponent<EnemyHealth>();
        muzzleflash = firepos.GetComponentInChildren<ParticleSystem>();
    }

    
    void Update()
    {
        if (EHealth.isdie)
            return;

        if (isCombat && !isReload && bossMove.lookPlayer)
        {
            if (Time.time >= nextFire)
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firepos.position, firepos.rotation);
        muzzleflash.Play();
        nextFire = Time.time + FireRate + Random.Range(0, 0);
        isReload = (--remainAmmo % rechargeAmmo == 0);
        if(isReload)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        pattern = Random.Range(0, 4);
        movepattern = true;
        yield return new WaitForSeconds(0.5f);
        ani.SetBool("IsMove", false);
        movepattern = false;
        yield return new WaitForSeconds(2.0f);
        remainAmmo = rechargeAmmo;
        isReload = false;
    }
}
