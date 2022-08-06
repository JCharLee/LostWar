using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    Transform playertr;
    Transform tr;
    Animator ani;
    public Transform firepos;
    GameObject bulletPrefab;
    EnemyHealth EHealth;
    ParticleSystem muzzleflash;
    private float nextfire = 0f;
    public readonly float firerate = 0.3f;
    public bool isFire = false;
    private int ammo = 10;
    private readonly int pullammo = 10;
    private bool reload = false;

    void Start()
    {
        bulletPrefab = Resources.Load<GameObject>("EBullet");
        playertr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        EHealth = GetComponent<EnemyHealth>();
        muzzleflash = firepos.GetComponentInChildren<ParticleSystem>();
    }

    
    void Update()
    {
        if (EHealth.isdie)
            return;
    }

    public void DoAttack()
    {
        if (!reload)
        {
            Quaternion rot = Quaternion.LookRotation(playertr.position - tr.position);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * 3.0f);
            if (Time.time >= nextfire)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        ani.SetTrigger("IsFire");
        setAccuracy();
        GameObject bullet = Instantiate(bulletPrefab, firepos.position, firepos.rotation);
        muzzleflash.Play();
        nextfire = Time.time + firerate + Random.Range(0f, 0f);
        reload = (--ammo % pullammo == 0);
        if (reload)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        ani.SetTrigger("IsReload");
        yield return new WaitForSeconds(3f);
        ammo = pullammo;
        reload = false;
    }

    void setAccuracy()
    {
        firepos.localEulerAngles =  new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f);
    }
}
