using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MouseLookX))]
public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 7;
    public float jumpHeight = 2;
    public int maxAdditionalJumps = 1;
    public float airSlow = 0.005f;  // Affects speed control in the air
    public float airSpeed = 0.001f;  // Reduces player speed in the air
    public float slidingSpeed = 12;
    public float maxSlidingTime = 2;
    public float slidingCooldown = 0.5f;
    public float slidingPlayerYScale = 0.4f;  // Player height during sliding
    public float getUpSpeed = 5f;  // Speed of getting player up after the sliding
    public float wallRunningSpeed = 10;
    public float wallRunningSpeedDown = -2;
    public float wallJumpPower = 3f;
    public float wallJumpHeight = 1.6f;
    // For how long player is invulnerable after a jump
    public float jumpInvulnerabilityTime = 0.7f;
    public float invulnerabilityTime;

    private bool groundedPlayer;
    private float groundedYspeed = -3f;  // Y speed while player is grounded
    private Vector3 playerVelocity;
    private int additionalJumps;
    private const float gravity = -9.8f;
    private float slidingTime;
    private Vector3 slidingDirection;
    private float slidingCooldownLeft;
    private float playerYScale;
    // List of walls which collide with the player (for wall-running and jumping)
    private List<Wall> collidingWalls;   
    private Vector3 wallRunningDirection;
    private Wall wallToRun;  // Wall that is used for wall-running

    private CharacterController controller;
    private AudioSource slidingAudio;
    private AudioSource jumpAudio;
    private AudioSource runAudio;


    public enum MovementType
    {
        Normal, Sliding, WallRunning
    }

    public MovementType movementType;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerYScale = transform.localScale.y;
        movementType = MovementType.Normal;
        collidingWalls = new List<Wall>();
        slidingAudio = transform.Find("SlidingAudio").GetComponent<AudioSource>();
        jumpAudio = transform.Find("JumpAudio").GetComponent<AudioSource>();
        runAudio = transform.Find("RunAudio").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Game.state != Game.State.Play)
            return;

        invulnerabilityTime -= TimeManager.deltaTime();
        slidingCooldownLeft -= TimeManager.deltaTime();
        
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = groundedYspeed;
            additionalJumps = 0;
        }

        switch (movementType)
        {
            case MovementType.Normal:
                HandleNormalMovement();
                break;
            case MovementType.Sliding:
                HandleSliding();
                break;
            case MovementType.WallRunning:
                HandleWallRunning();
                break;
        }
        
        controller.Move(playerVelocity * TimeManager.deltaTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Wall wall = collision.gameObject.GetComponent<Wall>();
            collidingWalls.Add(wall);
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            float dist = collision.transform.position.y - transform.position.y;
            if (dist >= 1 && playerVelocity.y > 0)
                playerVelocity.y = 0;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Wall wall = other.gameObject.GetComponent<Wall>();
            collidingWalls.Remove(wall);
        }
    }

    private void HandleNormalMovement()
    {
        Vector3 move = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        if (groundedPlayer && move.z > 0 && slidingCooldownLeft <= 0 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartSliding(transform.TransformDirection(move));
            return;
        }

        if (!groundedPlayer && move.z > 0 && CanWallRun())
        {
            StartWallRunning();
            return;
        }
        
        move = transform.TransformDirection(move.normalized * playerSpeed);

        if (groundedPlayer)
        {
            playerVelocity.x = move.x;
            playerVelocity.z = move.z;
        }
        else
        {
            // apply player controls
            Vector3 airMove = move;
            airMove *= airSlow;
            if (TimeManager.slow)
                airMove *= TimeManager.slowCoefficient;
            airMove += playerVelocity;
            airMove.y = 0;
            airMove = Vector3.ClampMagnitude(airMove, playerSpeed);

            // apply air slowing
            Vector3 airInfluence = new Vector3(-Mathf.Abs(airMove.x) * airMove.x, 0, 
                -Mathf.Abs(airMove.z) * airMove.z) * airSpeed;
            if (TimeManager.slow)
                airInfluence *= TimeManager.slowCoefficient;
            Vector3 newMove = airMove + airInfluence;
            if (newMove.x * airMove.x < 0)
                newMove.x = 0;
            if (newMove.z * airMove.z < 0)
                newMove.z = 0;
            
            playerVelocity.x = newMove.x;
            playerVelocity.z = newMove.z;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            if (groundedPlayer)
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                invulnerabilityTime = jumpInvulnerabilityTime;
                if (Game.soundsOn)
                    jumpAudio.Play();
            }
            else if (CanWallJump())
            {
                Vector3 jumpDir = WallJumpDirection();
                playerVelocity += jumpDir * wallJumpPower;
                playerVelocity.y = Mathf.Sqrt(wallJumpHeight * -2.0f * gravity);
                invulnerabilityTime = jumpInvulnerabilityTime;
                if (Game.soundsOn)
                    jumpAudio.Play();
            }
            else if (additionalJumps < maxAdditionalJumps)
            {
                if (playerVelocity.y < 0)
                {
                    playerVelocity = move;
                    playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                }
                else
                {
                    float dv = Mathf.Sqrt(playerVelocity.y * playerVelocity.y + jumpHeight * -2.0f * gravity) -
                               playerVelocity.y;
                    float newYvelocity = playerVelocity.y + dv;
                    playerVelocity = move;
                    playerVelocity.y = newYvelocity;
                }
                    
                additionalJumps += 1;
                invulnerabilityTime = jumpInvulnerabilityTime;
                if (Game.soundsOn)
                    jumpAudio.Play();
            }
                
        }

        playerVelocity.y += gravity * TimeManager.deltaTime();
    }

    private void StartSliding(Vector3 direction)
    {
        slidingDirection = direction.normalized;
        slidingTime = 0;
        transform.localScale = new Vector3(transform.localScale.x, slidingPlayerYScale, transform.localScale.x);
        movementType = MovementType.Sliding;
        if (Game.soundsOn)
            slidingAudio.Play();
    }
    
    private void HandleSliding()
    {
        Vector3 move = slidingDirection * slidingSpeed;
        playerVelocity.x = move.x;
        playerVelocity.z = move.z;
        
        playerVelocity.y += gravity * TimeManager.deltaTime();

        slidingTime += TimeManager.deltaTime();
        if (slidingTime >= maxSlidingTime || Input.GetKeyDown(KeyCode.LeftShift))
            StopSliding();
    }
    
    private void StopSliding()
    {
        movementType = MovementType.Normal;
        slidingCooldownLeft = slidingCooldown;
        StartCoroutine(GetUp());
        if (slidingAudio.isPlaying)
            slidingAudio.Stop();
    }

    // Gets the player up after the sliding
    private IEnumerator GetUp()
    {
        while (movementType == MovementType.Normal && transform.localScale.y < playerYScale)
        {
            if (Game.state != Game.State.Play)
            {
                yield return null;
                continue;
            }
            
            transform.localScale = new Vector3(transform.localScale.x, 
                transform.localScale.y + getUpSpeed * TimeManager.deltaTime(), 
                transform.localScale.z);
            yield return null;
        }

        transform.localScale = new Vector3(transform.localScale.x, playerYScale, transform.localScale.z);

    }

    private bool CanWallJump()
    {
        Vector3 dir = transform.forward;
        dir.y = 0;
        
        foreach (Wall wall in collidingWalls)
        {
            Vector3 normal = wall.GetNormal();
            if ((Vector3.Angle(dir, -normal) < 15) || 
                (Vector3.Angle(dir, normal) < 15))
            {
                return true;
            }
        }

        return false;
    }

    private Vector3 WallJumpDirection()
    {
        Vector3 dir = transform.forward;
        dir.y = 0;
        
        foreach (Wall wall in collidingWalls)
        {
            Vector3 normal = wall.GetNormal();
            if ((Vector3.Angle(dir, -normal) < 15) ||
                (Vector3.Angle(dir, normal) < 15))
            {
                return normal;
            }
        }
        
        return Vector3.zero;
    }

    private bool CanWallRun()
    {
        foreach (Wall wall1 in collidingWalls)
        {
            foreach (Wall wall2 in collidingWalls)
            {
                if (Vector3.Angle(wall1.GetNormal(), wall2.GetNormal()) < 135 &&
                    Vector3.Angle(wall1.GetNormal(), wall2.GetNormal()) > 45)
                {
                    return false;
                }
            } 
        }
        
        Vector3 dir = transform.forward;
        dir.y = 0;
        
        foreach (Wall wall in collidingWalls)
        {
            Vector3 normal = wall.GetNormal();
            if (Vector3.Angle(dir, -normal) > 45 && Vector3.Angle(dir, -normal) < 90)
            {
                wallRunningDirection = Vector3.ProjectOnPlane(dir, normal).normalized;
                wallToRun = wall;
                return true;
            }
        }

        return false;
    }

    private void HandleWallRunning()
    {
        if (groundedPlayer || Input.GetAxisRaw("Vertical") <= 0)
            StopWallRunning();

        if (!collidingWalls.Contains(wallToRun))
        {
            bool found = false;
            foreach (Wall wall in collidingWalls)
            {
                if (wallToRun.GetNormal() == wall.GetNormal())
                {
                    wallToRun = wall;
                    found = true;
                    break;                    
                }
            }
            if (!found)
                StopWallRunning();
        }
        
        foreach (Wall wall in collidingWalls)
        {
            if (Vector3.Angle(wallToRun.GetNormal(), wall.GetNormal()) < 135 && 
                Vector3.Angle(wallToRun.GetNormal(), wall.GetNormal()) > 45)
            {
                StopWallRunning();                   
            }
        }

        Vector3 move = wallRunningDirection * wallRunningSpeed;
        playerVelocity.x = move.x;
        playerVelocity.z = move.z;
        
        playerVelocity.y = wallRunningSpeedDown;
    }

    private void StartWallRunning()
    {
        playerVelocity.y = 0;
        movementType = MovementType.WallRunning;
        if (Game.soundsOn)
        {
            runAudio.loop = true;
            if (!runAudio.isPlaying)
                runAudio.Play();
        }
    }

    private void StopWallRunning()
    {
        movementType = MovementType.Normal;
        if (runAudio.isPlaying)
            runAudio.loop = false;
    }
}
