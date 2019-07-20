using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float distanceToGround;
    public float jumpVelocity;
    [HideInInspector] public bool frozen = false;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Move and jump, if not frozen
    void FixedUpdate()
    {
        if (!frozen)
        {
            CheckMovement();
            CheckJump();
        }
        else
        {
            GetComponent<Animator>().SetBool("isWalking", false);
        }
    }

    // Checks if jump conditions are met, and if so, initiates jump.
    private void CheckJump()
    {
        if (IsGrounded(rb.position, distanceToGround) && Input.GetButton("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        }
    }

    // Checks if movement conditions are met, and if so, moves.
    private void CheckMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        bool xMovementIsGrounded = IsGrounded(rb.position + new Vector3(speed * horizontalAxis, 0) * Time.deltaTime, Mathf.Infinity);
        bool zMovementIsGrounded = IsGrounded(rb.position + new Vector3(0, 0, speed * verticalAxis) * Time.deltaTime, Mathf.Infinity);
        bool totalMovementIsGrounded = IsGrounded(rb.position + new Vector3(speed * horizontalAxis, 0, speed * verticalAxis) * Time.deltaTime, Mathf.Infinity);

        if (totalMovementIsGrounded)
        {
            Vector3 movement = new Vector3(speed * horizontalAxis, 0, speed * verticalAxis) * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            RotateAndAnimateWalking(movement);
        }
        else if (xMovementIsGrounded)
        {
            Vector3 movement = new Vector3(speed * horizontalAxis, 0, 0) * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            RotateAndAnimateWalking(movement);
        }
        else if (zMovementIsGrounded)
        {
            Vector3 movement = new Vector3(0, 0, speed * verticalAxis) * Time.deltaTime;
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
            GetComponent<Animator>().SetBool("isWalking", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("isWalking", false);
        }
    }

    //Checks if an object exists at most dist down from position.
    bool IsGrounded(Vector3 position, float dist)
    {
        return Physics.Raycast(position, -Vector3.up, dist);
    }
}
