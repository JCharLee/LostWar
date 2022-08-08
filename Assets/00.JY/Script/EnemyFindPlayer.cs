using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFindPlayer : MonoBehaviour
{
    private Transform tr;
    private NavMeshAgent enemyAgent;
    [SerializeField]
    public Transform playerTr;
    private Animator ani;
    EnemyPatrol enemyPatrol;
    EnemyFire enemyFire;
    EnemyHealth EHealth;
    private float combatDist = 10f;
    private float traceDist = 15f;
    bool IsCombat = false;
    //private float patrolDist = 20f;
    int layermask;

    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        tr = GetComponent<Transform>();
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyFire = GetComponent<EnemyFire>();
        ani = GetComponent<Animator>();
        EHealth = GetComponent<EnemyHealth>();
        layermask = 1 << 0 | 1 << 9;
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 dir = (playerTr.position + playerTr.up * 1f - tr.position).normalized;
        Debug.DrawRay(tr.position, dir * traceDist, Color.green);
        if (Physics.Raycast(tr.position, dir, out hit, traceDist, layermask))
        {
            enemyFire.isFire = (hit.collider.CompareTag("Player"));
        }
        else
            enemyFire.isFire = false;
    }

    void FixedUpdate()
    {
        if (EHealth.isdie)
            return;

        float dist = Vector3.Distance(playerTr.position, tr.position);
        ani.SetFloat("Speed", enemyAgent.speed);

        if (dist <= combatDist && enemyFire.isFire)
        {
            //여기서 공격할지 피할지 구현
            IsCombat = true;
            enemyFire.DoAttack();
            enemyAgent.isStopped = true;
            ani.SetBool("IsMove", false);
        }
        else if (dist <= combatDist && !enemyFire.isFire && IsCombat)
        {
            enemyAgent.isStopped = false;
            ani.SetBool("IsMove", true);
        }
        else if (dist <= traceDist && enemyFire.isFire)
        {
            IsCombat = false;
            enemyAgent.speed = 7f;
            enemyAgent.destination = playerTr.position;
            enemyAgent.isStopped = false;
            ani.SetBool("IsMove", true);
        }
        else if (dist <= traceDist && !enemyFire.isFire)
        {
            IsCombat = false;
            enemyPatrol.patrol();
        }
        else if (dist >= traceDist)
        {
            //패트롤 할 부분
            IsCombat = false;
            enemyPatrol.patrol();
        }
    }
}
