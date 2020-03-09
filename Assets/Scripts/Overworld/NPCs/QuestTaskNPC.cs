using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTaskNPC : NPC
{
    public List<string> Dialog = new List<string>();
    public bool HasRepeatDialog;
    public List<string> RepeatDialog = new List<string>();
    [HideInInspector] private QuestgiverNPC quest;

    public override void HandleButtonPress()
    {
        if (HasRepeatDialog && TalkedTo)
        {
            DialogController.Instance.HandleDialogPress(RepeatDialog, null);
        }
        else
        {
            DialogController.Instance.HandleDialogPress(Dialog, null);
            TalkedTo = true;
            CompleteTask();
        }
    }

    //Increment the task counter for the parent quest. Called when the task is completed and when the scene is loaded.
    public void CompleteTask()
    {
        quest.TasksCompleted++;
    }
}
