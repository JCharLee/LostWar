using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2phaseEnter : MonoBehaviour
{
    GameObject Boss2phase;
    GameObject plasmaexp;

    void Start()
    {
        Boss2phase = Resources.Load<GameObject>("Boss2Phase");
        plasmaexp = Resources.Load<GameObject>("PlasmaExp");
        StartCoroutine(SpawnBoss2phase());
    }

    IEnumerator SpawnBoss2phase()
    {
        yield return new WaitForSeconds(2.0f);
        Instantiate(plasmaexp, transform.position, transform.rotation);
        Instantiate(Boss2phase, transform.position, transform.rotation);
    }
    
    void Update()
    {
        
    }
}
