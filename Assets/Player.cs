using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputManager inputManager;
    private Rigidbody2D rb;



    private bool isGrounded = true;
    private bool jumpPressed = false;
    private float jumpStartTime;

    [SerializeField]
    private Animator animator;




    // collisions and raycasting

    [SerializeField]
    private Transform groundDetectOrigin;

    [SerializeField]
    private LayerMask landableLayers;



    // velocity, gravity and acceleration

    private float velocity = 0;

    [SerializeField]
    [Tooltip("The velocity immediately applied upon pressing jump")]
    [Range(0.5f, 100f)]
    private float instantJumpVelocity = 5f;

    [SerializeField]
    [Tooltip("The acceleration constantly applied upon pressing jump")]
    [Range(0.5f, 100f)]
    private float jumpAcceleration = 0.5f;

    [SerializeField]
    [Tooltip("The maximum velocity the player may move at - terminal velocity")]
    [Range(0.5f, 100f)]
    private float maxVelocity = 2f;

    [SerializeField]
    [Tooltip("The maximum time that the player can be in the air for")]
    [Range(0.5f, 100f)]
    private float maxJumpTime = 2f;

    [SerializeField]
    [Tooltip("The gravitational force being applied downwards constantly")]
    [Range(0.5f, 100f)]
    private float gravity = 2f;






    void Start()
    {
        inputManager = InputManager.instance;
        inputManager.onTouchDown.AddListener(StartJump);
        inputManager.onTouchUp.AddListener(StopJump);

        GameManager.instance.onStartGame.AddListener(() =>
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("Running", true);
        });

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

    }



    // when jump is pressed. Start jumping if currently on the ground.
    private void StartJump()
    {
        if (isGrounded)
        {
            jumpPressed = true;
            velocity = instantJumpVelocity;
            jumpStartTime = Time.time;
            animator.SetBool("Falling", false);
            animator.SetBool("Jumping", true);
            Debug.Log("Start Jump");
        }
    }

    // When jump has stopped being pressed
    private void StopJump()
    {
        jumpPressed = false;
        Debug.Log("Stop Jump");
    }



    private void FixedUpdate()
    {
        rb.velocity = new Vector2(0, velocity);
    }




    private void Update()
    {

        // detect if the player is on the ground
        RaycastHit2D hit = Physics2D.Raycast(groundDetectOrigin.position, Vector2.down, 0.1f, landableLayers);

        if (hit)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // for the jump time, while the jump button is held apply a smaller constant force
        if (jumpPressed && (jumpStartTime + maxJumpTime) > Time.time)
        {
            velocity += jumpAcceleration * Time.deltaTime;
        }

        // this occurs when the player is falling or running
        else
        {
            // if the player has any velocity other than 0, they are falling
            if (Mathf.Abs(rb.velocity.y) > 0.01)
            {
                animator.SetBool("Falling", true);
                animator.SetBool("Jumping", false);
            }
            else
            {
                animator.SetBool("Falling", false);
                animator.SetBool("Jumping", false);
            }
        }

        // the player is not on the ground
        if (!isGrounded)
        {
            // apply gravity
            velocity -= gravity * Time.deltaTime;

            // limit the players velocity
            if (Mathf.Abs(velocity) >= maxVelocity)
            {
                if (velocity < 0)
                    velocity = -maxVelocity;
                else if (velocity > 0)
                    velocity = maxVelocity;
            }

        }
        else
        {
            if(!jumpPressed)
                velocity = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundDetectOrigin.position, (Vector2)groundDetectOrigin.position + Vector2.down * 0.1f);

    }
}
