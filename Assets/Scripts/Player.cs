using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public int playerId;
    public readonly static float speed = 8;
    public readonly static float maxHorizontalSpeed = 10;
    public readonly static float jumpSpeed = 15;

    private float currentSpeed;

    private readonly static float gravity = 25F;

    public static Player player1;
    public static Player player2;

    private Player otherPlayer;

    private Vector3 startPos;

    private CharacterController controller;

    private Action[] SkillActions;
    private static Player[] SkillOwner;

    private AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip slowMotionClip;

    public static void OnSceneReload()
    {
        SkillOwner = null;
    }

    void Init()
    {
        transform.position = startPos;
        SkillActions = new Action[]{ SlowDown, Jump };
        currentSpeed = speed;
        if (SkillOwner == null)
        {
            SkillOwner = new Player[]{ this, null };
            SkillDisplay.OnSkillOwnerChanged(0, this);
        }
        else
        {
            SkillOwner[1] = this;
            SkillDisplay.OnSkillOwnerChanged(1, this);
            if (playerId == 1)
                otherPlayer = player2;
            else
                otherPlayer = player1;
            otherPlayer.otherPlayer = this;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            audioSource.PlayOneShot(jumpClip);
        }
    }

    private static float slowDownTime = 2;
    private static float slowDownFactor = 0.3f;
    private float slowDownPressedTime = 0;

    private void SlowDown()
    {
        if (slowDownPressedTime == 0)
        {
            slowDownPressedTime = Time.time;
            audioSource.PlayOneShot(slowMotionClip);
        }
    }

    private void UpdateSlowDown()
    {
        if (slowDownPressedTime != 0)
        {
            float dt = Time.time - slowDownPressedTime;
            if (dt > 3 * slowDownTime)
            {  
                currentSpeed = speed;
                slowDownPressedTime = 0;
            }
            else if (dt < slowDownTime)
                currentSpeed = speed * slowDownFactor;
            else
                currentSpeed = speed * (3 - slowDownFactor) / 2;
        }
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

    private Vector3 moveDirection;

    void FixedUpdate()
    {
        if (!GameHandler.IsRunning())
            return;

        CheckActions();
        moveDirection.x = currentSpeed;
        moveDirection.y -= gravity * Time.fixedDeltaTime;
        controller.Move(moveDirection * Time.fixedDeltaTime);

        UpdateSlowDown();
    }

    void CheckActions()
    {
        for (int i = 0; i != SkillActions.Length; ++i)
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
        else if (hit.gameObject.GetComponent<KillCollision>())
        {
            GameHandler.GameOver();
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
