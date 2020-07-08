using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNPC : NPC
{
    public List<string> Dialog = new List<string>();
    public Item ItemToGive;
    public bool HasRepeatDialog;
    public List<string> RepeatDialog = new List<string>();

    public override void HandleDialogBegin()
    {
        if (HasRepeatDialog && TalkedTo)
        {
            DialogController.Instance.HandleDialogBegin(RepeatDialog, null);
        }
        else
        {
            DialogController.Instance.HandleDialogBegin(Dialog, ItemToGive);
            TalkedTo = true;
        }
    }
}
