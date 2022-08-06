using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCam : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float height = 8.0f;
    public float moveDamping = 15f;
    public float rotDamping = 15f;
    private float y;
    private float yMin = -90f;
    private float yMax = 90f;

    void LateUpdate()
    {
        y -= Input.GetAxis("Mouse Y") * rotDamping * Time.deltaTime;
        y = Mathf.Clamp(y, yMin, yMax);

        Vector3 camTr = target.position - (target.forward * distance) + (target.up * height);
        transform.position = Vector3.Slerp(transform.position, camTr, moveDamping * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotDamping * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(y, 0f, 0f);
        transform.LookAt(target.position + (target.up * 0.2f));

    }
}
