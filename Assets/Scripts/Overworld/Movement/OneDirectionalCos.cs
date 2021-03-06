﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirectionalCos : MonoBehaviour
{
    public Vector3 Displacement;
    public float BeatsPerPeriod;

    const float RADIANS_PER_CIRCLE = 2 * Mathf.PI;

    private Vector3 startingPosition;
    private Vector3 lastDisplacement;

    void Start()
    {
        startingPosition = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        float period = (MusicMaster.Instance.GetSecondsPerBeat() * BeatsPerPeriod);
        float time = MusicMaster.Instance.GetPlaybackTime();

        float sinInput;
        if (period == 0)
        {
            sinInput = time;
        }
        else
        {
            sinInput = (time / period) * RADIANS_PER_CIRCLE;
        }

        float x = startingPosition.x + Mathf.Cos(sinInput) * Displacement.x;
        float y = startingPosition.y + Mathf.Cos(sinInput) * Displacement.y;
        float z = startingPosition.z + Mathf.Cos(sinInput) * Displacement.z;
        lastDisplacement = new Vector3(x, y, z) - transform.position;
        transform.Translate(lastDisplacement, Space.World);
    }
}
