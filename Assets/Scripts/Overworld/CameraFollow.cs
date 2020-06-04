using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float lerpSpeedX;
    [SerializeField] private float lerpSpeedY;
    [SerializeField] private float lerpSpeedZ;

    [SerializeField] private float yDistAbovePlayer;
    [SerializeField] private float zDistFromPlayer;
    [SerializeField] private GameObject player;

    [SerializeField] private Transform defaultRotation;

    [HideInInspector] public bool IsFollowing;

    private void Start()
    {
        //Start following the player unless the player is frozen, in which case they loaded into a cutscene
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Frozen)
        {
            IsFollowing = true;
        }
    }

    void FixedUpdate()
    {
        if (IsFollowing)
        {
            Vector3 cameraPos = transform.position;
            Vector3 playerPos = player.transform.position;
            float x = Mathf.Lerp(cameraPos.x, playerPos.x, lerpSpeedX * Time.deltaTime);
            float y = Mathf.Lerp(cameraPos.y, playerPos.y + yDistAbovePlayer, lerpSpeedY * Time.deltaTime);
            float z = Mathf.Lerp(cameraPos.z, playerPos.z - zDistFromPlayer, lerpSpeedZ * Time.deltaTime);
            transform.position = new Vector3(x, y, z);
            transform.rotation = defaultRotation.transform.rotation;
        }
    }
}
