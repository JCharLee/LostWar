using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    [SerializeField]
    List<Transform> Nodes;
    
    void Start()
    {
        var Point = transform.GetComponent<Transform>();
        if(Point != null)
        {
            Point.GetComponentsInChildren<Transform>(Nodes);
            Nodes.RemoveAt(0);
        }
    }

    
    void Update()
    {
        
    }
}
