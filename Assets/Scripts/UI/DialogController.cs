using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : Singleton<DialogController>
{
    public GameObject DialogPanel;
    public Text DialogText;
    public float TimeBetweenCharacters;

    private bool isDisplayingDialog = false;
    private bool isRevealingChars = false;
    private int charsToDisplay = 0;
    private string stringToDisplay;
    public Player player;
    private Queue<string> currentDialog;

    //Disable dialog box, get player
    private void Start()
    {
        DialogPanel.SetActive(false);
        DialogText.text = "";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //Handle dialog button being pressed around an NPC with dialog dialog
    public void HandleDialogPress(List<string> dialog)
    {
        if (!isDisplayingDialog)
        {
            BeginDialog(dialog);
        }
        else
        {
            if (isRevealingChars)
            {
                MaximizeCharsToDisplay();
            }
            else
            {
                ShowNextLine();
            }

        }
    }

    //If no dialog is being displayed, start displaying the provided text
    private void BeginDialog(List<string> dialog)
    {
        player.frozen = true;
        if (!isDisplayingDialog)
        {
            isDisplayingDialog = true;
            currentDialog = new Queue<string>(dialog);
            DialogPanel.SetActive(true);
            charsToDisplay = 0;
            ShowNextLine();
        }
    }

    //Handle displaying the line of dialog atop the queue, or ending if the queue is empty
    private void ShowNextLine()
    {
        StopCoroutine(IncreaseCharsToDisplay());
        charsToDisplay = 0;
        isRevealingChars = true;

        if (currentDialog.Count > 0)
        {
            stringToDisplay = currentDialog.Dequeue();
            StartCoroutine(IncreaseCharsToDisplay());
        } else {
            EndDialog();
        }
    }

    //Close the dialog window and resume normal gameplay
    private void EndDialog()
    {
        player.frozen = false;
        isDisplayingDialog = false;
        DialogPanel.SetActive(false);
        DialogText.text = "";
    }

    //Display all characters, as long as there's a stringToDisplay
    private void MaximizeCharsToDisplay()
    {
        charsToDisplay = stringToDisplay.Length;
        isRevealingChars = false;
        StopCoroutine(IncreaseCharsToDisplay());
    }

    //Display one more character
    private IEnumerator IncreaseCharsToDisplay()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenCharacters);
            if (charsToDisplay < stringToDisplay.Length)
            {
                charsToDisplay++;
            }
            else
            {
                isRevealingChars = false;
            }
            DialogText.text = stringToDisplay.Substring(0, charsToDisplay);
        }
    }
}
