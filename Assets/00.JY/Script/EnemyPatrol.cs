using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody rigid;
    private CapsuleCollider capcol;
    private Transform tr;
    private NavMeshAgent agent;
    private Animator ani;
    [SerializeField]
    private int currentNode = 0;
    [SerializeField]
    List<Transform> Nodes;
    public GameObject PatrolPoint;

    float arrivetime = Mathf.Infinity;
    bool isarrive = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        capcol = GetComponent<CapsuleCollider>();
        tr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        var point = PatrolPoint.transform;
        if(point != null)
        {
            point.GetComponentsInChildren<Transform>(Nodes);
            Nodes.RemoveAt(0);
        }
    }
    
    void Update()
    {
        arrivetime += Time.deltaTime;
    }

    public void patrol()
    {
        if (Vector3.Distance(tr.position, Nodes[currentNode].position) < 1f)
        {
            ani.SetBool("IsMove", false);
            if (!isarrive)
                arrivetime = 0;
            isarrive = true;
            if (arrivetime > 3f)
            {
                CheckPoint();
            }
        }
        else
            Move();
    }

    public void Move()
    {
        ani.SetBool("IsMove", true);
        agent.isStopped = false;
        agent.destination = Nodes[currentNode].position;
        agent.speed = 3f;
    }

    private void CheckPoint()
    {
        currentNode = Random.Range(0, Nodes.Count);
        isarrive = false;
    }
}
