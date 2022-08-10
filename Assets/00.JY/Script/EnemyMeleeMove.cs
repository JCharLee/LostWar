using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeMove : MonoBehaviour
{
    NavMeshAgent agent;
    Animator ani;
    Transform tr;
    Transform playerTr;
    new AudioSource audio;
    EnemyPatrol Epatrol;
    EnemyHealth EHealth;
    public Collider sword;
    float attacktime = 0f;
    float nextattack1time = 2.26f;
    float nextattack2time = 2.4f;
    float nextattack3time = 3.1f;
    [SerializeField]
    bool isaction = false;
    readonly string attack = "IsAttack";
    [SerializeField]
    int attacknum = 0;
    int layermask;
    float traceDist = 20f;
    bool LookPlayer = false;
    bool IsCombat = false;
    AudioClip swingsfx;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        Epatrol = GetComponent<EnemyPatrol>();
        EHealth = GetComponent<EnemyHealth>();
        audio = GetComponent<AudioSource>();
        swingsfx = Resources.Load<AudioClip>("Sound/Swing1-Free-1");
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        layermask = 1 << 9 | 1 << 8 | 1 << 0;

        agent.speed = 8f;
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 dir = (playerTr.position - tr.position).normalized;
        Debug.DrawRay(tr.position + tr.up * 1, dir * traceDist, Color.green);
        if (Physics.Raycast(tr.position + tr.up * 1, dir, out hit, traceDist, layermask))
            LookPlayer = (hit.collider.CompareTag("Player"));
        else
            LookPlayer = false;
    }

    void FixedUpdate()
    {
        if (EHealth.isdie || FindObjectOfType<Health>().isdie)
            return;

        float dist = Vector3.Distance(playerTr.position, tr.position);
        attacktime += Time.deltaTime;

        if(dist <= 1.5f && LookPlayer)
        {
            IsCombat = true;
            ani.SetBool("IsMove", false);
            agent.isStopped = true;
            RandomAttack();
        }
        if (dist <= 1.5f && LookPlayer && IsCombat)
        {
            agent.speed = 6f;
            ani.SetBool("IsMove", true);
            agent.isStopped = false;
        }
        else if(dist <= traceDist && LookPlayer)
        {
            agent.speed = 6f;
            IsCombat = false;
            if (!isaction)
            {
                ani.SetBool("IsMove", true);
                agent.destination = playerTr.position;
                agent.isStopped = false;
            }
        }
        else if (dist <= traceDist && !LookPlayer)
        {
            agent.speed = 6f;
            IsCombat = false;
            Epatrol.patrol();
        }
        else
        {
            IsCombat = false;
            Epatrol.patrol();
        }
    }

    IEnumerator Cooltime(float cooltime)
    {
        isaction = true;
        yield return new WaitForSeconds(cooltime);
        sword.enabled = false;
        isaction = false;
    }

    void RandomAttack()
    {
        switch(attacknum)
        {
            case 0:
                Attackmove(nextattack1time, attacknum);
                break;
            case 1:
                Attackmove(nextattack2time, attacknum);
                break;
            case 2:
                Attackmove(nextattack3time, attacknum);
                break;
        }
    }

    void attacksfx()
    {
        audio.PlayOneShot(swingsfx, 1f);
    }

    void Attackmove(float nextattack, int attack_num)
    {
        if(attacktime >= nextattack)
        {
            Quaternion rot = Quaternion.LookRotation(playerTr.position - tr.position);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * 50.0f);

            sword.enabled = true;

            ani.SetTrigger(attack);
            ani.SetInteger("AttackIdx",attack_num);
            Invoke("attacksfx", 0.9f);
            StartCoroutine(Cooltime(nextattack));
            attacknum = Random.Range(0, 3);
            attacktime = 0;
        }
    }
}
