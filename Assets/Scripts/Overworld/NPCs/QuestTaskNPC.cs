using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTaskNPC : NPC
{
    public List<string> InactiveQuestDialog = new List<string>();
    public List<string> QuestDialog = new List<string>();
    public List<string> QuestRepeatDialog = new List<string>();
    [SerializeField] private string quest;

    public override void HandleDialogBegin()
    {
        if (!PlayerStats.CheckQuestStarted(quest))
        {
            DialogController.Instance.HandleDialogBegin(InactiveQuestDialog, null);
        }
        else if (TalkedTo)
        {
            DialogController.Instance.HandleDialogBegin(QuestRepeatDialog, null);
        }
        else
        {
            DialogController.Instance.HandleDialogBegin(QuestDialog, null);
            CompleteTask();
        }
    }

    //Increment the task counter for the parent quest and mark that this NPC has already advanced their quest.
    public void CompleteTask()
    {
        TalkedTo = true;
        PlayerStats.IncrementQuestCounter(quest);
    }
}
