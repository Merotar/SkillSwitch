using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerId;
    public static float acceleration = 20;
    public static float maxHorizontalSpeed = 10;
    public static float jumpSpeed = 10;

    private float distToGround;

    private Rigidbody rb;

    public static Player player1;
    public static Player player2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var collider = GetComponent<Collider>();
        distToGround = collider.bounds.extents.y;
        if (playerId == 1)
            player1 = this;
        else
            player2 = this;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal" + playerId);
        bool jump = Input.GetButtonDown("Jump" + playerId);

        Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);

        Vector3 velo = rb.velocity;

        if (velo.x > maxHorizontalSpeed)
        {
            velo.x = maxHorizontalSpeed;
            movement = Vector3.zero;
        }

        rb.AddForce(movement * acceleration);

        if (jump && IsGrounded())
            velo += jumpSpeed * Vector3.up;

        rb.velocity = velo; 
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
