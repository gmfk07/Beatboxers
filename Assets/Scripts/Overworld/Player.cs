using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpVelocity;
    public float DistanceToCheckGround;
    [HideInInspector] public bool frozen = false;

    private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Move and jump, if not frozen
    void FixedUpdate()
    {
        if (!frozen)
        {
            CheckMovement();
            CheckGrounded();
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    // Checks if grounded, handling jumps and falling animations.
    private void CheckGrounded()
    {
        if (IsGrounded(rb.position, DistanceToCheckGround))
        {
            animator.SetBool("isGrounded", true);
            if (Input.GetButton("Jump"))
            {
                rb.velocity = new Vector3(rb.velocity.x, JumpVelocity, rb.velocity.z);
                animator.SetTrigger("jump");
                animator.SetBool("isGrounded", false);
            }
            else
            {
                animator.ResetTrigger("jump");
                animator.SetBool("isGrounded", true);
            }
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    // Checks if movement conditions are met, and if so, moves.
    private void CheckMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        bool xMovementIsGrounded = IsGrounded(rb.position + new Vector3(Speed * horizontalAxis, 0) * Time.deltaTime, Mathf.Infinity);
        bool zMovementIsGrounded = IsGrounded(rb.position + new Vector3(0, 0, Speed * verticalAxis) * Time.deltaTime, Mathf.Infinity);
        bool totalMovementIsGrounded = IsGrounded(rb.position + new Vector3(Speed * horizontalAxis, 0, Speed * verticalAxis) * Time.deltaTime, Mathf.Infinity);

        if (totalMovementIsGrounded)
        {
            Vector3 movement = new Vector3(Speed * horizontalAxis, 0, Speed * verticalAxis) * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            RotateAndAnimateWalking(movement);
        }
        else if (xMovementIsGrounded)
        {
            Vector3 movement = new Vector3(Speed * horizontalAxis, 0, 0) * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            RotateAndAnimateWalking(movement);
        }
        else if (zMovementIsGrounded)
        {
            Vector3 movement = new Vector3(0, 0, Speed * verticalAxis) * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            RotateAndAnimateWalking(movement);
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(rb.position + Vector3.up * .2f, .2f);
    }

    //Checks if an object exists at most dist down from the player at position.
    bool IsGrounded(Vector3 position, float dist)
    {
        float xExtent = GetComponent<Collider>().bounds.extents.x;
        float zExtent = GetComponent<Collider>().bounds.extents.z;

        bool centerHit = Physics.Raycast(position + Vector3.up * .2f, Vector3.down, dist);
        bool leftHit = Physics.Raycast(position + Vector3.left * xExtent, Vector3.down, dist);
        bool rightHit = Physics.Raycast(position + Vector3.right * xExtent, Vector3.down, dist);
        bool frontHit = Physics.Raycast(position + Vector3.forward * zExtent, Vector3.down, dist);
        bool backHit = Physics.Raycast(position + Vector3.back * zExtent, Vector3.down, dist);

        return centerHit || leftHit || rightHit || frontHit || backHit;
    }
}
