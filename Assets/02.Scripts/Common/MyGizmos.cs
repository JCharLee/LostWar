using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color _color;
    public float radius;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color.linear;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
