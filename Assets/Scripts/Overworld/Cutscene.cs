using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public enum Transition { Cut, Pan };

    //All the five lists must be the same length!
    [SerializeField] private List<List<string>> dialogs; //The list of dialog strings in between shots. Empty list = no dialog.
    [SerializeField] private List<Transform> transforms; //The different Transforms the camera occupies in different shots. Null = no change from previous position.
    [SerializeField] private List<Transition> transitions; //The transitions to use when moving to the shot. If this shot's Transform is same as prev, this is irrelevant.
    [SerializeField] private List<float> panTimes; //How long a pan transition should take. If this shot's transition isn't Pan, this is irrelevant.

    [SerializeField] private DialogController dialogController; //The DialogController to make cutscene calls to
    [SerializeField] private Player player; //The Player to freeze/unfreeze

    [HideInInspector] public bool hasTriggered = false;

    private bool transitionComplete = false;
    private bool dialogComplete = false;

    void Start()
    {
        //Enforce equal-length lists
        if (dialogs.Count != transforms.Count || transforms.Count != transitions.Count ||
            transitions.Count != panTimes.Count && panTimes.Count != panTimes.Count)
        {
            Debug.LogError("Malformed cutscene - lists of unequal length");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCutscene();
        }
    }

    //Freezes the player and begins the cutscene with a transition to shot 0.
    private void StartCutscene()
    {
        player.Frozen = true;
    }

    //Attempts to transition to the given shot index, returning true if this is valid and false if not
    private bool TryTransitionToShot(int shotIndex)
    {
        if (shotIndex < dialogs.Count)
        {
            return false;
        }
        else
        {
            if (transitions[shotIndex] == Transition.Cut)
            {
                Camera.main.transform.SetPositionAndRotation(transforms[shotIndex].position, transforms[shotIndex].rotation);
                transitionComplete = true;
            }
            else if (transitions[shotIndex] == Transition.Pan)
            {
                StartCoroutine(Pan(transforms[shotIndex], panTimes[shotIndex]));
            }
        }

        return true;
    }

    IEnumerator Pan(Transform newTransform, float panTime)
    {
        Vector3 displacement = Camera.main.transform.position - newTransform.position;
        Vector3 displacementPerSecond = displacement / panTime;
        float startTime = Time.time;
        while (Time.time - startTime < panTime)
        {
            Camera.main.transform.Translate(displacementPerSecond * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
