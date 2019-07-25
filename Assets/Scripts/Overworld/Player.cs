using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpVelocity;
    public float DistanceToCheckGround;
    public float Gravity;
    public float MaxJumpHoldTime;
    [HideInInspector] public bool Frozen = false;

    private Animator animator;
    private CharacterController cc;
    private float yVelocity;
    private bool isJumping = false;
    private float startedJumpingTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Move and jump, if not frozen
    void FixedUpdate()
    {
        if (!Frozen)
        {
            HandleGrounding();
            CheckMovement();
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    // Checks if grounded, handling jumps and falling animations.
    private void HandleGrounding()
    {
        if (IsGrounded(cc.transform.position, DistanceToCheckGround))
        {
            animator.SetBool("isGrounded", true);
            if (Input.GetButton("Jump"))
            {
                yVelocity = JumpVelocity;
                animator.SetTrigger("jump");
                animator.SetBool("isGrounded", false);
                isJumping = true;
                startedJumpingTime = Time.time;
            }
            else
            {
                yVelocity = 0;
                animator.ResetTrigger("jump");
                animator.SetBool("isGrounded", true);
            }
        }
        else
        {
            if (isJumping && Input.GetButton("Jump") && Time.time - startedJumpingTime <= MaxJumpHoldTime)
            {
                yVelocity = JumpVelocity;
            }
            else
            {
                isJumping = false;
            }
            animator.SetBool("isGrounded", false);
        }
    }

    // Checks if movement conditions are met, and if so, moves.
    private void CheckMovement()
    {
        yVelocity -= Gravity * Time.deltaTime;

        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(Speed * horizontalAxis, yVelocity, Speed * verticalAxis) * Time.deltaTime;

        if (IsGrounded(cc.transform.position + movement, Mathf.Infinity, .3f))
        {
            cc.Move(movement);
        }
        else
        {
            movement = Vector3.up * yVelocity * Time.deltaTime;
            cc.Move(movement);
        }

        RotateAndAnimateWalking(new Vector3(horizontalAxis, 0, verticalAxis));
    }

    //Rotate in the movement direction and start the walking animation if movement is nonzero, otherwise, stop the animation.
    private void RotateAndAnimateWalking(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    //Checks if an object exists at most dist down from the player at position.
    bool IsGrounded(Vector3 position, float dist, float fractionOfRadius = 1)
    {
        float xExtent = GetComponent<Collider>().bounds.extents.x;
        float zExtent = GetComponent<Collider>().bounds.extents.z;

        bool centerHit = Physics.Raycast(position + Vector3.up * .2f, transform.TransformDirection(Vector3.down), dist);
        bool leftHit = Physics.Raycast(position + Vector3.left * xExtent * fractionOfRadius, transform.TransformDirection(Vector3.down), dist);
        bool rightHit = Physics.Raycast(position + Vector3.right * xExtent * fractionOfRadius, transform.TransformDirection(Vector3.down), dist);
        bool frontHit = Physics.Raycast(position + Vector3.forward * zExtent * fractionOfRadius, transform.TransformDirection(Vector3.down), dist);
        bool backHit = Physics.Raycast(position + Vector3.back * zExtent * fractionOfRadius, transform.TransformDirection(Vector3.down), dist);

        return centerHit || leftHit || rightHit || frontHit || backHit;
    }
}
