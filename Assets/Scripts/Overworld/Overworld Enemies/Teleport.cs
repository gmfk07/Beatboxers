using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private List<Transform> teleportSpots = new List<Transform>();
    [SerializeField] private float secondsBetweenTeleport;
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private float timeBeforeTeleportToAnimate;

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

    //Sends the trigger "TeleportIncoming", which should begin playing modelAnimator's pre-teleport animation.
    private void BeginPreTeleportAnimation()
    {

    }

    //Sends the trigger "Teleport", which should begin playing modelAnimator's on-teleport animation.
    private void BeginTeleportAnimation()
    {

    }

    //Updates this GameObject's transform based on its current teleportSpotIndex
    private void UpdateTransform()
    {
        transform.SetPositionAndRotation(teleportSpots[teleportSpotIndex].position, teleportSpots[teleportSpotIndex].rotation);
    }
}
