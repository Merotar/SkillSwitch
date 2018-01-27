using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public int playerId;
    public static float speed = 8;
    public static float maxHorizontalSpeed = 10;
    public static float jumpSpeed = 15;

    private static float gravity = 25F;

    public static Player player1;
    public static Player player2;

    private Player otherPlayer;

    private Vector3 startPos;

    private CharacterController controller;

    private Action[] SkillActions;
    private static Player[] SkillOwner;


    void Init()
    {
        transform.position = startPos;
        SkillActions = new Action[]{ Shoot,  Jump, Slide, Run };
        if (SkillOwner == null)
        {
            SkillOwner = new Player[]{ this, this, null, null };
            SkillDisplay.OnSkillOwnerChanged(0, this);
            SkillDisplay.OnSkillOwnerChanged(1, this);
        }
        else
        {
            SkillOwner[2] = SkillOwner[3] = this;
            SkillDisplay.OnSkillOwnerChanged(2, this);
            SkillDisplay.OnSkillOwnerChanged(3, this);
            if (playerId == 1)
                otherPlayer = player2;
            else
                otherPlayer = player1;
            otherPlayer.otherPlayer = this;
        }
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
        Init();
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
            if (Input.GetButton("Skill" + i + "_" + playerId))
            {
                OnSkillTriggered(i);
            }
        }
    }

    void OnSkillTriggered(int skillId)
    {
        if (OwnsSkill(skillId))
        {
            if (Input.GetButton("Shift_" + playerId))
                SwapSkill(skillId);
            else
                SkillActions[skillId]();
        }
    }

    bool OwnsSkill(int skillId)
    {
        return SkillOwner[skillId] == this;
    }

    void SwapSkill(int skillId)
    {
        SkillOwner[skillId] = SkillOwner[skillId].otherPlayer;
        SkillDisplay.OnSkillOwnerChanged(skillId, SkillOwner[skillId]);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Goal>())
        {
            GameHandler.OnPlayerReachedGoal(this);
        }
        else
        {
            if ((controller.collisionFlags & CollisionFlags.Sides) != 0)
            {
                Debug.Log(Time.frameCount + "\t" + hit.point);
            }
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

    public Vector3 StartPosition()
    {
        return startPos;
    }
}
