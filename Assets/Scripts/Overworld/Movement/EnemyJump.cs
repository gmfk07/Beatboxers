﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    [SerializeField] private float jumpLengthInBeats;
    [SerializeField] private float jumpPauseInBeats;
    [SerializeField] private float jumpHeight;

    private float yStart;
    private float jumpLengthInSeconds;
    private float jumpPauseInSeconds;
    private float gravity; //Negative change in velocity per second
    private float yVelocity;
    private bool jumping;

    //yVelocity = -(gravity * jumpLengthInSeconds) / 2;
    //gravity = -Mathf.Pow(yVelocity, 2) / (2 * jumpHeight);

    private void Start()
    {
        yStart = transform.position.y;
        jumpLengthInSeconds = jumpLengthInBeats * MusicMaster.Instance.GetSecondsPerBeat();
        jumpPauseInSeconds = jumpPauseInBeats * MusicMaster.Instance.GetSecondsPerBeat();
        jumping = false;
        gravity = -8 * jumpHeight / Mathf.Pow(jumpLengthInSeconds, 2);
        Invoke("Jump", MusicMaster.Instance.GetPlaybackTime() % (jumpPauseInSeconds + jumpLengthInSeconds));
        yVelocity = 0;
    }

    //Set yVelocity to jump up to jumpHeight under the influence of gravity, landing back at startingY after jumpLengthInBeats
    private void Jump()
    {
        yVelocity = 4 * jumpHeight/jumpLengthInSeconds;
        jumping = true;
        Invoke("StopJump", jumpLengthInSeconds);
    }

    //Update position with yVelocity, then changes yVelocity based on gravity.
    private void FixedUpdate()
    {
        if (jumping)
        {
            float newY = transform.position.y + yVelocity * Time.deltaTime;
            Vector3 newPos = new Vector3(transform.position.x, newY, transform.position.z);
            transform.position = newPos;
            yVelocity += gravity * Time.deltaTime;
        }
    }

    //End the jump and start the timer for the next jump.
    private void StopJump()
    {
        Vector3 startPos = new Vector3(transform.position.x, yStart, transform.position.z);
        transform.position = startPos;

        jumping = false;
        Invoke("Jump", jumpPauseInSeconds);
    }
}
