using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Animator anim;
    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        if (anim != null)
            anim.SetBool("IsMoving", moveInput.sqrMagnitude > 0.01f);
    }

    void FixedUpdate()
    {
        Vector3 velocity = moveInput * moveSpeed;
        velocity.y = 0f; // lock Y explicitly

        rb.linearVelocity = velocity;
    }

}

