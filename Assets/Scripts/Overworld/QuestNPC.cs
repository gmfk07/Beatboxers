using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : NPC
{
    private List<string> startDialog;
    private List<string> ongoingDialog;
    private List<string> completionDialog;
    private List<string> finishedDialog;

    public override void HandleButtonPress()
    {
        return;
    }
}
