﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListWrapper
{
    public List<string> dialog;
}

public class Cutscene : MonoBehaviour
{
    public enum Transition { Cut, Pan };

    //All the five lists must be the same length!
    [SerializeField] private List<ListWrapper> dialogs; //The list of dialogue strings in between shots. Empty list = no dialog.
    [SerializeField] private List<Transform> transforms; //The different Transforms the camera occupies in different shots. Null = no change from previous position.
    [SerializeField] private List<Transition> transitions; //The transitions to use when moving to the shot. If this shot's Transform is same as prev, this is irrelevant.
    [SerializeField] private List<float> panTimes; //How long a pan transition should take. If this shot's transition isn't Pan, this is irrelevant.

    [SerializeField] private DialogController dialogController; //The DialogController to make cutscene calls to
    [SerializeField] private Player player; //The Player to freeze/unfreeze

    private bool inDialog = false;
    private int currentShotIndex = -1;

    [HideInInspector] public bool HasTriggered = false;

    void Start()
    {
        //Enforce equal-length lists
        if (dialogs.Count != transforms.Count || transforms.Count != transitions.Count ||
            transitions.Count != panTimes.Count && panTimes.Count != panTimes.Count)
        {
            Debug.LogError("Malformed cutscene - lists of unequal length");
        }
    }

    //Begin the cutscene if we haven't triggered it yet!
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !HasTriggered)
        {
            HasTriggered = true;
            StartCutscene();
        }
    }

    private void Update()
    {
        if (inDialog)
        {
            if (Input.GetButtonDown("Interact"))
            {
                dialogController.HandleDialogPress(dialogs[currentShotIndex].dialog);
            }

            if (!player.Frozen)
            {
                GoToShot(currentShotIndex + 1);
            }
        }
    }

    //Freezes the player and begins the cutscene with a transition to shot 0.
    private void StartCutscene()
    {
        currentShotIndex = 0;
        GoToShot(0);
    }

    //Attempts to transition to the given shot index, returning true if this is valid and false if not
    private bool TryTransitionToShot(int shotIndex)
    {
        if (shotIndex >= dialogs.Count)
        {
            return false;
        }
        else
        {
            if (transitions[shotIndex] == Transition.Cut)
            {
                Camera.main.transform.SetPositionAndRotation(transforms[shotIndex].position, transforms[shotIndex].rotation);
                StartDialog(shotIndex);
            }
            else if (transitions[shotIndex] == Transition.Pan)
            {
                StartCoroutine(Pan(shotIndex));
            }
        }
        return true;
    }

    //Attempts to start the dialog of a given shot index. If no dialog exists for that shot, marks the dialog as already over.
    private void StartDialog(int shotIndex)
    {
        if (dialogs[shotIndex] != null)
        {
            dialogController.HandleDialogPress(dialogs[shotIndex].dialog);
            inDialog = true;
        }
        else
        {
            GoToShot(shotIndex);
        }
    }

    //Go to the shot indexed by shotIndex, resetting the inDialog flag and refreezing the player. If no such shot exists, exit the cutscene.
    private void GoToShot(int shotIndex)
    {
        inDialog = false;
        if (!TryTransitionToShot(shotIndex))
        {
            //End the cutscene
            player.Frozen = false;
        }
    }

    IEnumerator Pan(int shotIndex)
    {
        Transform newTransform = transforms[shotIndex];
        float panTime = panTimes[shotIndex];

        Vector3 displacement = Camera.main.transform.position - newTransform.position;
        Vector3 displacementPerSecond = displacement / panTime;
        Vector3 rotateDiff = Camera.main.transform.rotation.eulerAngles - newTransform.rotation.eulerAngles;
        Vector3 rotatePerSecond = rotateDiff / panTime;
        float startTime = Time.time;

        while (Time.time - startTime < panTime)
        {
            Camera.main.transform.Translate(displacementPerSecond * Time.fixedDeltaTime);
            Camera.main.transform.Rotate(rotatePerSecond * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        Camera.main.transform.position = newTransform.position;
        Camera.main.transform.rotation = newTransform.rotation;

        StartDialog(shotIndex);
    }
}
