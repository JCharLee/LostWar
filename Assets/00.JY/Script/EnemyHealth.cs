using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float hpinit = 100f;
    public int exp;
    float healthpoint;
    public bool isdie = false;
    ParticleSystem bloodeff;
    NavMeshAgent agent;
    Animator ani;
    new AudioSource audio;
    [SerializeField]
    Image Hpbar;
    GameObject HpBarCanvas;
    Rigidbody rigid;
    CapsuleCollider capcol;
    AudioClip hitsfx;

    private void OnEnable()
    {
        healthpoint = hpinit;
        Hpbar = transform.Find("HP_Canvas").transform.GetChild(1).GetComponent<Image>();
        HpBarCanvas = transform.Find("HP_Canvas").gameObject;
        Hpbar.color = Color.green;
        audio = GetComponent<AudioSource>();
        hitsfx = Resources.Load<AudioClip>("Sound/BluntImpact6-Free-1");
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        capcol = GetComponent<CapsuleCollider>();
        bloodeff = transform.Find("BloodSprayEffect").GetComponent<ParticleSystem>();
    }


    void Update()
    {
        
    }

    public void takeDamage(float damage, Vector3 hitpoint)
    {
        if (isdie)
            return;

        healthpoint = Mathf.Clamp(healthpoint - damage, 0f, hpinit);
        bloodeff.transform.position = hitpoint;
        bloodeff.transform.rotation = Quaternion.LookRotation(hitpoint);
        bloodeff.Play();
        ani.SetTrigger("IsHit");
        audio.PlayOneShot(hitsfx, 1f);

        Hpbar.fillAmount = healthpoint / hpinit;


        if (healthpoint <= hpinit * 0.3)
            Hpbar.color = Color.red;
        else if (healthpoint <= hpinit * 0.7)
            Hpbar.color = Color.yellow;


        if (healthpoint <= 0)
        {
            //Debug.Log("die");
            if (ani != null)
                ani.SetTrigger("IsDie");
            isdie = true;
            agent.enabled = false;
            if (!QuestManager.instance.boss1Action)
                PlayerInteraction.instance.Kill();
            UIManager.instance.UpdateExp(exp);
            if (!QuestManager.instance.boss1Action && !QuestManager.instance.boss2Action)
                Instantiate(Resources.Load<GameObject>("Prefabs/DropItem"), this.transform.position, Quaternion.identity);
            rigid.Sleep();
            capcol.enabled = false;
            HpBarCanvas.SetActive(false);
            StopAllCoroutines();
        }
    }
}
