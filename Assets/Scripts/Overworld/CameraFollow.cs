using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float lerpSpeedX;
    [SerializeField] private float lerpSpeedZ;
    [SerializeField] private float zDistFromPlayer;
    [SerializeField] private GameObject player;

    void FixedUpdate()
    {
        Vector3 cameraPos = transform.position;
        Vector3 playerPos = player.transform.position;
        float x = Mathf.Lerp(cameraPos.x, playerPos.x, lerpSpeedX*Time.deltaTime);
        float z = Mathf.Lerp(cameraPos.z, playerPos.z - zDistFromPlayer, lerpSpeedZ*Time.deltaTime);
        transform.position = new Vector3(x, cameraPos.y, z);
    }
}
