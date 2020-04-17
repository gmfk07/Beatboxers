using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTaskNPC : NPC
{
    public List<string> InactiveQuestDialog = new List<string>();
    public List<string> QuestDialog = new List<string>();
    public bool HasRepeatDialog;
    public List<string> QuestRepeatDialog = new List<string>();
    [SerializeField] private string quest;

    public override void HandleButtonPress()
    {
        if (!PlayerStats.CheckQuestStarted(quest))
        {
            DialogController.Instance.HandleDialogPress(InactiveQuestDialog, null);
        }
        else if (HasRepeatDialog && TalkedTo)
        {
            DialogController.Instance.HandleDialogPress(QuestRepeatDialog, null);
        }
        else
        {
            DialogController.Instance.HandleDialogPress(QuestDialog, null);
            TalkedTo = true;
            CompleteTask();
        }
    }

    //Increment the task counter for the parent quest.
    public void CompleteTask()
    {
        PlayerStats.IncrementQuestCounter(quest);
    }
}
