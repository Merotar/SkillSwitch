using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float acceleration = 10;
    public float maxHorizontalSpeed = 10;
    public float jumpSpeed = 10;

    private float distToGround;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var collider = GetComponent<Collider>();
        distToGround = collider.bounds.extents.y;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");

        Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);
        rb.AddForce(movement * acceleration);
        if (jump && IsGrounded())
            rb.velocity += jumpSpeed * Vector3.up;

        rb.velocity = new Vector3(Mathf.Min(rb.velocity.x, maxHorizontalSpeed), rb.velocity.y); 
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
