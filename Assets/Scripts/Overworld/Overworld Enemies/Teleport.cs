using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private List<Transform> teleportSpots = new List<Transform>();
    [SerializeField] private float secondsBetweenTeleport;
    private int teleportSpotIndex = 0;

    private void Start()
    {
        UpdateTransform();
        InvokeRepeating("TeleportToNextSpot", secondsBetweenTeleport, secondsBetweenTeleport);
    }

    //Teleport to the next spot in teleportSpots and update position, resetting teleportSpotIndex when the end of the list is reached.
    private void TeleportToNextSpot()
    {
        teleportSpotIndex++;
        teleportSpotIndex = teleportSpotIndex % teleportSpots.Count;
        UpdateTransform();
    }

    //Updates this GameObject's transform based on its current teleportSpotIndex
    private void UpdateTransform()
    {
        transform.SetPositionAndRotation(teleportSpots[teleportSpotIndex].position, teleportSpots[teleportSpotIndex].rotation);
    }
}
