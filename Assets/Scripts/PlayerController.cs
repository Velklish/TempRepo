using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float gravityScale;
    
    //private Rigidbody rb;
    private CharacterController _controller;
    public Animator animator;

    private Vector3 moveDirection;
    private float knockBackCounter;
    public float knockbackTime;
    public float knockBackForce;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockBackCounter <= 0)
        {
            float yStore = moveDirection.y;
            moveDirection = transform.forward * Input.GetAxis("Vertical") +
                            transform.right * Input.GetAxis("Horizontal");
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            if (_controller.isGrounded)
            {
                moveDirection.y = 0f;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                }
            }
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }

        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        _controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }


        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")));
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void KnockBack(Vector3 direction)
    {
        knockBackCounter = knockbackTime;
        moveDirection = direction * knockBackForce;
    }
}
