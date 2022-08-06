using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float hpinit = 100f;
    float healthpoint;
    public bool isdie = false;
    ParticleSystem bloodeff;
    NavMeshAgent agent;
    Animator ani;
    [SerializeField]
    Image Hpbar;

    private void OnEnable()
    {
        healthpoint = hpinit;
        Hpbar = transform.Find("HP_Canvas").transform.GetChild(1).GetComponent<Image>();
        Hpbar.color = Color.green;
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
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
            PlayerInteraction.instance.Kill();
            StopAllCoroutines();
        }
    }
}
