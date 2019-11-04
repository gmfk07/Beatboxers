using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    [SerializeField] private float jumpLengthInBeats;
    [SerializeField] private float jumpPauseInBeats;
    [SerializeField] private float jumpHeight;

    private float yStart;
    private float yVelocity;
    private float jumpLengthInSeconds;
    private float jumpPauseInSeconds;
    private float gravity; //Negative change in velocity per second
    private bool jumping;

    private void Start()
    {
        yStart = transform.position.y;
        yVelocity = 0;
        jumpLengthInSeconds = jumpLengthInBeats * MusicMaster.Instance.SecondsPerBeat;
        jumpPauseInSeconds = jumpPauseInBeats * MusicMaster.Instance.SecondsPerBeat;
        jumping = false;
        gravity = -Mathf.Pow(yVelocity, 2) / (2 * jumpHeight);
        Invoke("Jump", jumpPauseInSeconds);
    }

    //Set yVelocity to jump up to jumpHeight under the influence of gravity, landing back at startingY after jumpLengthInBeats
    private void Jump()
    {
        Debug.Log(gravity);
        yVelocity = -(gravity * jumpLengthInSeconds) / 2;
        jumping = true;
    }

    //Update position with yVelocity, then changes yVelocity based on gravity.
    private void FixedUpdate()
    {
        if (jumping)
        {
            float newY = transform.position.y + yVelocity;
            transform.position.Set(transform.position.x, newY, transform.position.z);
            yVelocity += Time.deltaTime * gravity;
            if (newY <= yStart)
            {
                transform.position.Set(transform.position.x, yStart, transform.position.z);
                jumping = false;
                Invoke("Jump", jumpPauseInSeconds);
            }
        }
    }
}
