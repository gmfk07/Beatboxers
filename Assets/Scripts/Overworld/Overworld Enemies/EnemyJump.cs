using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    private float startingY;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float gravityAccel; //change in velocity per second

    private void Start()
    {
        startingY = transform.position.y;
    }
}
