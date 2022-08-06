using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeMove : MonoBehaviour
{
    NavMeshAgent agent;
    Animator ani;
    Transform tr;
    Transform playertr;
    EnemyPatrol Epatrol;
    EnemyHealth Ehealth;
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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        Epatrol = GetComponent<EnemyPatrol>();
        Ehealth = GetComponent<EnemyHealth>();
        playertr = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    
    void FixedUpdate()
    {
        if (Ehealth.isdie)
            return;

        float dist = Vector3.Distance(playertr.position, tr.position);
        attacktime += Time.deltaTime;

        if(dist <= 1.5f)
        {
            ani.SetBool("IsMove", false);
            agent.isStopped = true;
            RandomAttack();
        }
        else if(dist <= 15f)
        {
            if (!isaction)
            {
                ani.SetBool("IsMove", true);
                agent.destination = playertr.position;
                agent.isStopped = false;
            }
        }
        else
        {
            ani.SetBool("IsMove", true);
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

    void Attackmove(float nextattack, int attack_num)
    {
        if(attacktime >= nextattack)
        {
            Quaternion rot = Quaternion.LookRotation(playertr.position - tr.position);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * 50.0f);

            sword.enabled = true;

            ani.SetTrigger(attack);
            ani.SetInteger("AttackIdx",attack_num);
            StartCoroutine(Cooltime(nextattack));
            attacknum = Random.Range(0, 3);
            attacktime = 0;
        }
    }
}
