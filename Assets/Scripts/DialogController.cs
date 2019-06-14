using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : Singleton<DialogController>
{
    public GameObject DialogPanel;

    private Queue<string> currentDialog;
    private bool isDisplayingDialog = false;

    //If no dialog is being displayed, start displaying the provided text
    public void BeginDialog(List<string> dialog)
    {
        isDisplayingDialog = true;
        currentDialog = new Queue<string>(dialog);
        DialogPanel.SetActive(true);
        //Show text
    }
}
