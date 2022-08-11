using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform tr;
    private Transform mainCameraTr;

    void Start()
    {
        tr = GetComponent<Transform>();
        mainCameraTr = Camera.main.transform;
    }

    void Update()
    {
        tr.LookAt(mainCameraTr);
    }
}
