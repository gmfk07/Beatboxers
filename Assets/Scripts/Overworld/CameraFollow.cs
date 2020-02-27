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

    [SerializeField] private Quaternion rotation;

    [HideInInspector] public bool IsFollowing;

    private void Start()
    {
        IsFollowing = true;
        rotation = transform.rotation;
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
            transform.rotation = rotation;
        }
    }
}
