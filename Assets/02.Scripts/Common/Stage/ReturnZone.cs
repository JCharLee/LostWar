using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnZone : MonoBehaviour
{
    private Transform startPoint;

    private void Awake()
    {
        startPoint = FindObjectOfType<StartPoint>().transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.transform.position = startPoint.position;
    }
}