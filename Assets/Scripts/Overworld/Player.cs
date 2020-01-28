using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpVelocity;
    public float DistanceToCheckGrounded;
    public float DistanceToCheckInBounds;
    public float Gravity;
    public float MaxJumpHoldTime;
    [HideInInspector] public bool Frozen = false;

    private Animator animator;
    private CharacterController cc;
    private float yVelocity;
    private bool isJumping = false;
    private float startedJumpingTime = 0;

    [SerializeField] private float fractionOfRadiusForInBoundsCheck;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Move and jump, stopping movement if frozen.
    void FixedUpdate()
    {
        HandleGrounding();
        CheckMovement();

        if (Frozen)
        {
            animator.SetBool("isWalking", false);
        }
    }

    // Checks if grounded, handling jumps and falling animations.
    private void HandleGrounding()
    {
        if (CheckGrounded())
        {
            animator.SetBool("isGrounded", true);
            if (Input.GetButton("Jump") && !Frozen)
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

        if (Frozen)
        {
            horizontalAxis = verticalAxis = 0;
        }

        Vector3 movement = new Vector3(Speed * horizontalAxis, yVelocity, Speed * verticalAxis) * Time.deltaTime;
        cc.Move(movement);
        bool isInBounds = CheckInBounds(fractionOfRadiusForInBoundsCheck);

        if (!isInBounds)
        {
            cc.Move(-movement);
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

    //Checks if the player is grounded.
    bool CheckGrounded()
    {
        Vector3 bottomOfCapsuleSpherePosition = new Vector3(cc.transform.position.x, cc.transform.position.y + cc.radius, cc.transform.position.z);

        return Physics.CheckCapsule(bottomOfCapsuleSpherePosition, bottomOfCapsuleSpherePosition + DistanceToCheckGrounded * Vector3.down, cc.radius);
    }

    //Checks if the player would be in bounds at their current position (aka, checks that radius * fractionOfRadius is above ground at some distance).
    bool CheckInBounds(float fractionOfRadius)
    {
        float xExtent = cc.bounds.extents.x;
        float zExtent = cc.bounds.extents.z;

        bool centerHit = Physics.Raycast(cc.transform.position, transform.TransformDirection(Vector3.down), DistanceToCheckInBounds);
        bool leftHit = Physics.Raycast(cc.transform.position + Vector3.left * xExtent * fractionOfRadius, transform.TransformDirection(Vector3.down), DistanceToCheckInBounds);
        bool rightHit = Physics.Raycast(cc.transform.position + Vector3.right * xExtent * fractionOfRadius, transform.TransformDirection(Vector3.down), DistanceToCheckInBounds);
        bool frontHit = Physics.Raycast(cc.transform.position + Vector3.forward * zExtent * fractionOfRadius, transform.TransformDirection(Vector3.down), DistanceToCheckInBounds);
        bool backHit = Physics.Raycast(cc.transform.position + Vector3.back * zExtent * fractionOfRadius, transform.TransformDirection(Vector3.down), DistanceToCheckInBounds);

        return centerHit && leftHit && rightHit && frontHit && backHit;
    }
}
