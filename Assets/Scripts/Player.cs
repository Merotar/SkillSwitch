using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private Action[] SkillActions;

    public void ResetPosition()
    {
        transform.position = startPos;
        SkillActions = new Action[]{ Jump, Shoot, Slide, Run };
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
    }

    private void Shoot()
    {
    }

    private void Slide()
    {
    }

    private void Run()
    {
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

        CheckActions();

        moveDirection.y -= gravity * Time.fixedDeltaTime;
        controller.Move(moveDirection * Time.fixedDeltaTime);
    }

    void CheckActions()
    {
        for (int i = 0; i != 4; ++i)
        {
            //if (Input.GetButtonDown("Shift_" + playerId))
            if (Input.GetButtonDown("Skill" + i + "_" + playerId))
            {
                Debug.Log(i);
                SkillActions[i]();
            }
        }
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
