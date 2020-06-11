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
    public float BouncePadVelocity;
    [HideInInspector] public bool Frozen = false;

    private Animator animator;
    private CharacterController cc;
    private float yVelocity;
    private bool isJumping = false;
    private float startedJumpingTime = 0;

    [SerializeField] private float fractionOfRadiusForInBoundsCheck;

    [SerializeField] private float plantBeatWindow;
    [SerializeField] private GameObject plantPlatform;
    [SerializeField] private int maxPlantGrowths;
    [SerializeField] private float growthPerPress;

    private int currentPlantGrowths = 0;
    private GameObject currentPlant;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentPlant = null;
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

        float beatOffset = MusicMaster.Instance.GetPlaybackTime() % MusicMaster.Instance.GetSecondsPerBeat();

        if (Input.GetButtonDown("Plant"))
        {
            if ((beatOffset <= plantBeatWindow || beatOffset >= MusicMaster.Instance.GetSecondsPerBeat() - plantBeatWindow))
            {
                if (!CheckGroundedOnLayer("PlantPlatform"))
                {
                    TryCreatePlantPlatform();
                }
                else
                {
                    TryGrowPlant();
                }
            }
            else
            {
                Destroy(currentPlant);
            }
        }
    }

    //If there are extra plantGrowths that can be performed, grow the plant.
    private void TryGrowPlant()
    {
        Debug.Log("Im tryna grow");

        if (currentPlantGrowths < maxPlantGrowths)
        {
            Vector3 scale = currentPlant.transform.localScale;
            currentPlant.transform.localScale = new Vector3(scale.x, scale.y + growthPerPress, scale.z);
            cc.Move(new Vector3(0, currentPlant.GetComponentInChildren<BoxCollider>().size.y * growthPerPress));
            currentPlantGrowths++;
        }
    }

    // Checks if grounded, handling jumps and falling animations.
    private void HandleGrounding()
    {
        if (CheckGrounded())
        {
            animator.SetBool("isGrounded", true);
            if (CheckGroundedOnLayer("BouncePad"))
            {
                BeginJump(BouncePadVelocity);
            }
            else if (Input.GetButton("Jump") && !Frozen)
            {
                BeginJump(JumpVelocity);
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
                yVelocity = Mathf.Max(JumpVelocity, yVelocity);
            }
            else
            {
                isJumping = false;
            }
            animator.SetBool("isGrounded", false);
        }
    }

    //Initiates a jump with a given y velocity.
    private void BeginJump(float yvel)
    {
        yVelocity = yvel;
        animator.SetTrigger("jump");
        animator.SetBool("isGrounded", false);
        isJumping = true;
        startedJumpingTime = Time.time;
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

    //If the player has collected enough sheet music and is grounded on a plant-layer object, make the plant platform.
    private void TryCreatePlantPlatform()
    {
        if (CheckGroundedOnLayer("Plant") && SheetMusicUnlockManager.Instance.PlantGrowUnlocked)
        {
            if (currentPlant != null)
            {
                Destroy(currentPlant);
            }
            currentPlant = Instantiate(plantPlatform, cc.transform.position, Quaternion.identity);
            cc.Move(new Vector3(0, plantPlatform.GetComponentInChildren<BoxCollider>().size.y, 0));
            currentPlantGrowths = 0;
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

    //Checks if the player is grounded.
    bool CheckGrounded()
    {
        Vector3 bottomOfCapsuleSpherePosition = new Vector3(cc.transform.position.x, cc.transform.position.y + cc.radius, cc.transform.position.z);

        return Physics.CheckCapsule(GetBottomOfCapsuleSpherePosition(), GetBottomOfCapsuleSpherePosition() + DistanceToCheckGrounded * Vector3.down, cc.radius);
    }

    //Checks if the player is grounded, checking only on a layer with a given name
    bool CheckGroundedOnLayer(string layerName)
    {
        return Physics.CheckCapsule(GetBottomOfCapsuleSpherePosition(), GetBottomOfCapsuleSpherePosition() + DistanceToCheckGrounded * Vector3.down, cc.radius, LayerMask.GetMask(layerName));
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

    //Returns the position of the sphere at the bottom of the capsule.
    Vector3 GetBottomOfCapsuleSpherePosition()
    {
        return new Vector3(cc.transform.position.x, cc.transform.position.y + cc.radius, cc.transform.position.z);
    }
}
