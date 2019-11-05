using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private List<Transform> teleportSpots = new List<Transform>();
    [SerializeField] private float beatsBetweenTeleport;
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private float timeBeforeTeleportToAnimate;

    private int teleportSpotIndex = 0;

    private void Start()
    {
        UpdateTransform();
        StartCoroutine("TeleportCycle");
    }

    private IEnumerator TeleportCycle()
    {
        while (!MusicMaster.Instance.Initialized)
        {
            yield return new WaitForEndOfFrame();
        }

        float secondsBetweenTeleport = MusicMaster.Instance.GetSecondsPerBeat() * beatsBetweenTeleport;

        yield return new WaitForSeconds(secondsBetweenTeleport - (MusicMaster.Instance.GetPlaybackTime() % secondsBetweenTeleport));

        //Main loop
        while (true)
        {
            yield return new WaitForSeconds(secondsBetweenTeleport - timeBeforeTeleportToAnimate);

            BeginPreTeleportAnimation();

            yield return new WaitForSeconds(timeBeforeTeleportToAnimate);

            TeleportToNextSpot();
            BeginTeleportAnimation();
        }
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
        modelAnimator.SetTrigger("TeleportIncoming");
    }

    //Sends the trigger "Teleport", which should begin playing modelAnimator's on-teleport animation.
    private void BeginTeleportAnimation()
    {
        modelAnimator.SetTrigger("Teleport");
    }

    //Updates this GameObject's transform based on its current teleportSpotIndex
    private void UpdateTransform()
    {
        transform.SetPositionAndRotation(teleportSpots[teleportSpotIndex].position, teleportSpots[teleportSpotIndex].rotation);
    }
}
