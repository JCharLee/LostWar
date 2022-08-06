using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Transform tr;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;

    private float h, v;
    private float moveSpeed = 3.0f;

    private readonly int hashSpeedX = Animator.StringToHash("SpeedX");
    private readonly int hashSpeedY = Animator.StringToHash("SpeedY");


    private void Awake()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        anim.SetFloat(hashSpeedX, h);
        anim.SetFloat(hashSpeedY, v);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
    }
}