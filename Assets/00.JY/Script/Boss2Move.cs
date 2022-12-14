using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss2Move : MonoBehaviour
{
    Rigidbody rigid;
    Transform tr;
    Animator ani;
    public Transform playerTr;
    NavMeshAgent agent;
    EnemyHealth EHealth;
    //int layermask;
    public bool lookPlayer = false;
    bool isaction = false;
    float jumpattack = 2f;
    float attack = 1.3f;
    float punch = 1.1f;
    float sinceattacktime = Mathf.Infinity;
    float patterntimer = 0f;
    [SerializeField]
    int patternNum = 999;
    float dist;
    public CapsuleCollider Boss2Claw;
    public CapsuleCollider Boss2Punch;
    GameObject smallexp;
    private AudioSource B2audio;
    private AudioClip B2jumpatksfx;
    private AudioClip punchsfx;
    private AudioClip crawsfx;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        EHealth = GetComponent<EnemyHealth>();
        smallexp = Resources.Load<GameObject>("SmallExplosion");
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        B2audio = GetComponent<AudioSource>();
        B2jumpatksfx = Resources.Load<AudioClip>("Sound/AntiMaterialRifle_3p_03");
        punchsfx = Resources.Load<AudioClip>("Sound/Swing4-Free-1");
        crawsfx = Resources.Load<AudioClip>("Sound/Slice-MetalClank2-Free-1");
        //layermask = 1 << 9 | 1 << 8 | 1 << 0;
        agent.isStopped = true;
        agent.speed = 6f;
    }

    void Update()
    {
        if (EHealth.isdie || FindObjectOfType<Health>().isdie)
        {
            return;
        }
        ////플레이어 감지 레이캐스트
        //RaycastHit hit;
        //Vector3 dir = (playerTr.position - tr.position).normalized;
        //Debug.DrawRay(tr.position + tr.up * 1, dir * 50f, Color.green);
        //if (Physics.Raycast(tr.position + tr.up * 1, dir, out hit, 50f, layermask))
        //    lookPlayer = (hit.collider.CompareTag("Player"));
        //else
        //    lookPlayer = false;

        //패턴타이머
        patterntimer += Time.deltaTime;
        if (patterntimer >= 2)
        {
            patterntimer = 0;
            patternNum = Random.Range(0, 3);
        }

        sinceattacktime += Time.deltaTime;

        agent.destination = playerTr.position;

        switch(patternNum)
        {
            case 0:
                jumpAttack();
                break;
            case 1:
                Attack();
                break;
            case 2:
                Punch();
                break;
        }
    }

    private bool Getdist(float dist2)
    {
        dist = Vector3.Distance(tr.position, playerTr.position);
        return dist <= dist2;
    }

    void jumpAttack()
    {
        if (!isaction)
        {
            agent.isStopped = false;
            ani.SetBool("IsMove", true);
        }
        if (Getdist(3))
        {
            ani.SetBool("IsMove", false);
            agent.isStopped = true;
            if (sinceattacktime > jumpattack)
            {
                SlerpLook(300);
                StartCoroutine(actioncooltime(jumpattack));
                StartCoroutine(JumpAttack());
                ani.SetTrigger("JumpAttack");
                sinceattacktime = 0;
            }
        }
    }

    void Attack()
    {
        SlerpLook(30);
        if (!isaction)
        {
            agent.isStopped = false;
            ani.SetBool("IsMove", true);
        }
        if (Getdist(2))
        {
            ani.SetBool("IsMove", false);
            agent.isStopped = true;
            if (sinceattacktime > attack)
            {
                Boss2Claw.enabled = true;
                StartCoroutine(actioncooltime(attack));
                ani.SetTrigger("Attack");
                B2audio.clip = crawsfx;
                B2audio.PlayDelayed(0.3f);
                sinceattacktime = 0;
            }
        }
    }
    void Punch()
    {
        SlerpLook(30);
        if (!isaction)
        {
            agent.isStopped = false;
            ani.SetBool("IsMove", true);
        }
        if (Getdist(1))
        {
            ani.SetBool("IsMove", false);
            agent.isStopped = true;
            if (sinceattacktime > punch)
            {
                Boss2Punch.enabled = true;
                StartCoroutine(actioncooltime(punch));
                ani.SetTrigger("Punch");
                B2audio.clip = punchsfx;
                B2audio.PlayDelayed(0.2f);
                sinceattacktime = 0;
            }
        }
    }
    private void SlerpLook(float damping)
    {
        Quaternion rot = Quaternion.LookRotation(playerTr.position - tr.position);
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
    }

    IEnumerator actioncooltime(float cool)
    {
        isaction = true;
        yield return new WaitForSeconds(cool);
        Boss2Claw.enabled = false;
        Boss2Punch.enabled = false;
        isaction = false;
    }
    IEnumerator JumpAttack()
    {
        yield return new WaitForSeconds(1.0f);
        var sexp = Instantiate(smallexp, tr.position, Quaternion.identity);
        B2audio.PlayOneShot(B2jumpatksfx, 0.5f);
        Destroy(sexp, 1.9f);
        Collider[] cols = Physics.OverlapSphere(tr.position, 3f);
        foreach(Collider col in cols)
        {
            var health = col.GetComponent<Health>();
            if(health != null)
            {
                health.takeDamage(150f);
            }
        }
    }
}
