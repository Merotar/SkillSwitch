using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public int playerId;
    public static float speed = 10;
    public static float maxHorizontalSpeed = 10;
    public static float jumpSpeed = 10;

    public float gravity = 9.81F;

    public static Player player1;
    public static Player player2;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (playerId == 1)
            player1 = this;
        else
            player2 = this;
    }

    private Vector3 moveDirection = speed * Vector3.right;

    void FixedUpdate()
    {
        if (controller.isGrounded)
        {
            if (Input.GetButton("Jump" + playerId))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.fixedDeltaTime;
        controller.Move(moveDirection * Time.fixedDeltaTime);
    }
}
