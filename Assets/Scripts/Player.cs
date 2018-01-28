using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public int playerId;
    public readonly static float speed = 10;
    public readonly static float maxHorizontalSpeed = 10;
    public readonly static float jumpSpeed = 20;

    private float currentSpeed;

    private readonly static float gravity = 50;

    public static Player player1;
    public static Player player2;

    private Player otherPlayer;

    private Vector3 startPos;
    private Vector3 moveDirection;

    private CharacterController controller;

    private Action[] SkillActions;
    private static Player[] SkillOwner;

    public ParticleSystem sparkParticles;
    public ParticleSystem deathParticles;

    public float shotDelay;
    private float shotDt;

    private AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip slowMotionClip;
    public AudioClip skillSwitchClip;
    public AudioClip explosionClip;
    public AudioClip successClip;

    public GameObject shot;
    public CapsuleCollider leftWheelCollider;
    public CapsuleCollider centerCollider;
    bool groundedByExtraColliders = false;

    public static void OnSceneReload()
    {
    }

    void Init()
    {
        shotDt = 0;
        transform.position = startPos;
        SkillActions = new Action[]{ Jump, SlowDown, Shoot };
        currentSpeed = speed;
        if (SkillOwner == null)
            SkillOwner = new Player[3];

        if (playerId == 1)
            player1 = this;
        else
            player2 = this;

        if (player1 && player2)
        {
            if (playerId == 1)
                otherPlayer = player2;
            else
                otherPlayer = player1;
            otherPlayer.otherPlayer = this;
        }

//        sparkParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        sparkParticles.Stop();
        sparkParticles.enableEmission = false;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            sparkParticles.Stop();
            sparkParticles.enableEmission = false;
            moveDirection.y = jumpSpeed;
            transform.position += 0.1f * Vector3.up;
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
        if (shotDt <= 0)
        {
            Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
            GameObject.Instantiate(shot, new Vector3(0.5f + renderer.bounds.size.x / 2.0f, -0.6f, 0) + gameObject.transform.position, Quaternion.identity);
            shotDt = shotDelay;
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

    void FixedUpdate()
    {
        if (shotDt > 0)
        {
            shotDt -= Time.fixedDeltaTime;
        }

        if (!GameHandler.IsRunning())
        {
            sparkParticles.Stop();
            sparkParticles.enableEmission = false;
            return;
        }
        else
        {
            if (sparkParticles.isStopped & IsGrounded())
            {
                sparkParticles.Play();
                sparkParticles.enableEmission = true;
            }
        }

        CheckActions();
        moveDirection.x = currentSpeed;
        moveDirection.y -= gravity * Time.fixedDeltaTime;

        groundedByExtraColliders = CheckExtraCollider(leftWheelCollider) || CheckExtraCollider(centerCollider);

        controller.Move(moveDirection * Time.fixedDeltaTime);

        UpdateSlowDown();
    }

    bool CheckExtraCollider(CapsuleCollider collider)
    {
        return CheckExtraCollider(collider, Vector3.up) || CheckExtraCollider(collider, Vector3.down);
    }

    bool CheckExtraCollider(CapsuleCollider collider, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + collider.center, direction, out hit))
        {
            float dy = (transform.position + collider.center - hit.point).y;
            if (Mathf.Abs(dy) <= collider.height / 2 + 0.05f && !CheckIfHitObjectIsSpecial(hit.collider.gameObject))
            {
                transform.position += (collider.height / 2 - dy) * Vector3.up;
                moveDirection.y = 0;
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
            var v3 = hit.transform.position - transform.position;
            var angle = Vector3.Angle(v3, transform.right);

            if (angle < 30)
            {
                InitDeath();
            }
        }
    }

    private bool CheckIfHitObjectIsSpecial(GameObject hitObject)
    {
        if (hitObject.GetComponent<Goal>())
        {
            GameHandler.OnPlayerReachedGoal(this);
        }
        else if (hitObject.GetComponent<KillCollision>())
            InitDeath();
        else if (hitObject.GetComponent<Skill>() == null)
            return false;
        return true;
    }


    public void InitDeath()
    {
        audioSource.PlayOneShot(explosionClip);
        deathParticles.Play();
        GameHandler.GameOver();
    }

    public Vector3 StartPosition()
    {
        return startPos;
    }

    public void GiveSkill(int skillId)
    {
        SkillOwner[skillId] = this;
    }
}
