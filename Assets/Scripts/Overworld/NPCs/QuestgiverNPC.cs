using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestgiverNPC : NPC
{
    [SerializeField] private List<string> startDialog;
    [SerializeField] private List<string> ongoingDialog;
    [SerializeField] private List<string> completionDialog;
    [SerializeField] private List<string> finishedDialog;

    [SerializeField] private List<GameObject> questRewardDeactivation;
    [SerializeField] private List<GameObject> questRewardActivations;

    [SerializeField] private string quest;

    public int tasksNeeded;
    [HideInInspector] public bool HasTurnedInQuest = false;

    public override void HandleButtonPress()
    {
        if (!TalkedTo)
        {
            DialogController.Instance.HandleDialogPress(startDialog, null);
            PlayerStats.StartQuest(quest);
            TalkedTo = true;
        }
        else if (TalkedTo && PlayerStats.QuestCounter[quest] < tasksNeeded && !HasTurnedInQuest)
        {
            DialogController.Instance.HandleDialogPress(ongoingDialog, null);
        }
        else if (TalkedTo && PlayerStats.QuestCounter[quest] >= tasksNeeded && !HasTurnedInQuest)
        {
            DialogController.Instance.HandleDialogPress(completionDialog, null);
            HasTurnedInQuest = true;
            GiveQuestRewards();
        }
        else if (HasTurnedInQuest)
        {
            DialogController.Instance.HandleDialogPress(finishedDialog, null);
        }
    }

    //Make relevant gameobjects appear and disappear once the quest is completed
    public void GiveQuestRewards()
    {
        foreach (GameObject go in questRewardActivations)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in questRewardDeactivation)
        {
            go.SetActive(false);
        }
    }
}
