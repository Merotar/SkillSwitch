using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public int playerId;
    public static float speed = 0;
    public static float maxHorizontalSpeed = 10;
    public static float jumpSpeed = 10;

    public float gravity = 9.81F;

    public static Player player1;
    public static Player player2;

    private Vector3 startPos;

    private CharacterController controller;

    public void ResetPosition()
    {
        transform.position = startPos;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (playerId == 1)
            player1 = this;
        else
            player2 = this;

        startPos = transform.position;
    }

    private Vector3 moveDirection = speed * Vector3.right;

    void FixedUpdate()
    {
        if (!GameHandler.IsRunning())
            return;
        if (controller.isGrounded)
        {
            if (Input.GetButton("Shift_" + playerId))
            if (Input.GetButtonDown("Skill1_" + playerId))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.fixedDeltaTime;
        controller.Move(moveDirection * Time.fixedDeltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Goal>())
        {
            GameHandler.OnPlayerReachedGoald(this);
        }
        else
        {
            CheckCollisions(controller.collisionFlags);
        }
    }

    private void CheckCollisions(CollisionFlags collisions)
    {
        if ((collisions & CollisionFlags.Sides) != 0)
            OnCollisionSides();
    }

    private void OnCollisionSides()
    {
        GameHandler.GameOver();    
    }
}
