using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNPC : NPC
{
    public List<string> Dialog = new List<string>();
    public Item ItemToGive;
    public bool HasRepeatDialog;
    public List<string> RepeatDialog = new List<string>();

    public override void HandleButtonPress()
    {
        if (HasRepeatDialog && TalkedTo)
        {
            DialogController.Instance.HandleDialogPress(RepeatDialog, null);
        }
        else
        {
            DialogController.Instance.HandleDialogPress(Dialog, ItemToGive);
            TalkedTo = true;
        }
    }
}
