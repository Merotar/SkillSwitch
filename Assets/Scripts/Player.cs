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
    public AudioClip skillSwitchClip;

    public GameObject shot;
    public CapsuleCollider leftWheelCollider;
    public CapsuleCollider centerCollider;
    bool groundedByExtraColliders = false;

    public static void OnSceneReload()
    {
        SkillOwner = null;
    }

    void Init()
    {
        transform.position = startPos;
        SkillActions = new Action[]{ Jump, SlowDown, Shoot };
        currentSpeed = speed;
        if (SkillOwner == null)
        {
            SkillOwner = new Player[]{ this, this, null };
            SkillDisplay.OnSkillOwnerChanged(0, this);
            SkillDisplay.OnSkillOwnerChanged(1, this);
        }
        else
        {
            SkillOwner[2] = this;
            SkillDisplay.OnSkillOwnerChanged(2, this);
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
        if (IsGrounded())
        {
            moveDirection.y = jumpSpeed;
            audioSource.PlayOneShot(jumpClip);
        }
    }

    private bool IsGrounded()
    {
        return controller.isGrounded || groundedByExtraColliders;
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

    private void Shoot()
    {
        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        GameObject.Instantiate(shot, new Vector3(0.5f + renderer.bounds.size.x / 2.0f, 0, 0) + gameObject.transform.position, Quaternion.identity);
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

        groundedByExtraColliders = CheckExtraCollider(leftWheelCollider) || CheckExtraCollider(centerCollider);
            
        controller.Move(moveDirection * Time.fixedDeltaTime);

        UpdateSlowDown();
    }

    bool CheckExtraCollider(CapsuleCollider collider)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + collider.center, -Vector3.up, out hit))
        {
            float dy = (transform.position + collider.center - hit.point).y;
            if (dy <= collider.height / 2)
            {
                transform.position += (collider.height / 2 - dy) * Vector3.up;
                moveDirection.y = 0;

                CheckIfHitObjectIsSpecial(hit.collider.gameObject);
                groundedByExtraColliders = true;
                return true;
            }
        }
        return false;
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
            {
                SwapSkill(skillId);
                audioSource.PlayOneShot(skillSwitchClip);
            }
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
        if (!CheckIfHitObjectIsSpecial(hit.gameObject))
        {
            CheckCollisions(controller.collisionFlags);
        }
    }

    private bool CheckIfHitObjectIsSpecial(GameObject hitObject)
    {
        if (hitObject.GetComponent<Goal>())
            GameHandler.OnPlayerReachedGoal(this);
        else if (hitObject.GetComponent<KillCollision>())
            GameHandler.GameOver();
        else
            return false;
        return true;
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
