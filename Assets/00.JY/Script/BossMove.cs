using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMove : MonoBehaviour
{
    Rigidbody rigid;
    Transform tr;
    Animator ani;
    public Transform playerTr;
    NavMeshAgent agent;
    EnemyHealth EHealth;
    BossCombat combat;
    GameObject rocketPrefab;
    int layermask;
    int rocketammo = 1;
    int dododge = 1;
    public bool lookPlayer = false;
    GameObject Boss2phase;
    GameObject plasmaexp;
    bool isboss2phase = false;
    private AudioSource BMaudio;
    private AudioClip BMmissile;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<BossCombat>();
        EHealth = GetComponent<EnemyHealth>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rocketPrefab = Resources.Load<GameObject>("Rocket");
        Boss2phase = Resources.Load<GameObject>("Boss2Phase");
        plasmaexp = Resources.Load<GameObject>("PlasmaExp");
        BMaudio = GetComponent<AudioSource>();
        BMmissile = Resources.Load<AudioClip>("Sound/JackHammer_1p_01");
        layermask = 1 << 9 | 1 << 8 | 1 << 0;
    }

    void Update()
    {
        if (EHealth.isdie)
        {
            if (!isboss2phase)
                StartCoroutine(Spawnboss2phase());
            return;
        }

        RaycastHit hit;
        Vector3 dir = (playerTr.position - tr.position).normalized;
        Debug.DrawRay(tr.position + tr.up * 1, dir * 50f, Color.green);
        if (Physics.Raycast(tr.position + tr.up * 1, dir, out hit, 50f, layermask))
            lookPlayer = (hit.collider.CompareTag("Player"));
        else
            lookPlayer = false;

        Quaternion rot = Quaternion.LookRotation(playerTr.position - tr.position);
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * 100.0f);
    }

    IEnumerator Spawnboss2phase()
    {
        isboss2phase = true;
        yield return new WaitForSeconds(4.0f);
        Destroy(this.gameObject);
        var pexp = Instantiate(plasmaexp, tr.position, tr.rotation);
        Instantiate(Boss2phase, tr.position, tr.rotation);
        QuestManager.instance.boss1Action = false;
        QuestManager.instance.boss2Action = true;
        Destroy(pexp, 1.9f);
    }
    void FixedUpdate()
    {
        if (EHealth.isdie)
            return;
        if (combat.isCombat)
            DoCombat();
    }

    private void DoCombat()
    {
        float dist = Vector3.Distance(tr.position, playerTr.position);
        if (lookPlayer && dist <= 10f)
        {
            ani.SetBool("IsMove", true);
            agent.isStopped = true;
            tr.position += tr.forward.normalized * -0.65f * Time.deltaTime;
            ani.SetFloat("SpeedY", -1f);
        }
        else if (!lookPlayer || dist >= 11f)
        {
            ani.SetBool("IsMove", true);
            agent.isStopped = false;
            agent.destination = playerTr.position;
            ani.SetFloat("SpeedY", 1f);
        }
        else
        {
            ani.SetBool("IsMove", false);
        }

        if (combat.movepattern)
        {
            switch (combat.pattern)
            {
                case 0:
                    if (dododge == 1)
                        StartCoroutine(DodgeAni());
                    tr.position += tr.right.normalized * 7.0f * Time.deltaTime;
                    break;
                case 1:
                    if (dododge == 1)
                        StartCoroutine(DodgeAni());
                    tr.position += tr.right.normalized * -7.0f * Time.deltaTime;
                    break;
                case 2:
                    if (rocketammo == 1)
                        StartCoroutine(rocketFire());
                    break;
                case 3:
                    ani.SetBool("IsMove", true);
                    ani.SetFloat("SpeedX", 1);
                    tr.position += tr.right.normalized * 2.0f * Time.deltaTime;
                    break;
                case 4:
                    ani.SetBool("IsMove", true);
                    ani.SetFloat("SpeedX", -1);
                    tr.position += tr.right.normalized * -2.0f * Time.deltaTime;
                    break;
            }
        }
    }

    IEnumerator rocketFire()
    {
        GameObject rocket = Instantiate(rocketPrefab, combat.firepos.position,
                        combat.firepos.rotation);
        BMaudio.PlayOneShot(BMmissile, 0.5f);
        --rocketammo;
        yield return new WaitForSeconds(0.5f);
        rocketammo = 1;
    }
    IEnumerator DodgeAni()
    {
        ani.SetTrigger("Dodge");
        --dododge;
        yield return new WaitForSeconds(0.5f);
        dododge = 1;
    }
}
