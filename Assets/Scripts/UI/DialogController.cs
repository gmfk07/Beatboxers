using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : Singleton<DialogController>
{
    public GameObject DialogPanel;
    public GameObject NewItemImage;
    public Text DialogText;
    public float TimeBetweenCharacters;

    private bool isDisplayingDialog = false;
    private bool isRevealingChars = false;
    private bool isGettingNewItem = false;
    private Item newItem;
    private int charsToDisplay = 0;
    private string stringToDisplay;
    private Player player;
    private Queue<string> currentDialog;

    //Disable dialog box, get player
    private void Start()
    {
        DialogPanel.SetActive(false);
        DialogText.text = "";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        NewItemImage.SetActive(false);
    }

    //Handle dialog button being pressed around an NPC with dialog dialog and giving item toGet
    public void HandleDialogPress(List<string> dialog, Item toGet = null)
    {
        if (!isDisplayingDialog)
        {
            BeginDialog(dialog, toGet);
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
    private void BeginDialog(List<string> dialog, Item toGet = null)
    {
        if (toGet != null)
        {
            isGettingNewItem = true;
            newItem = toGet;
        }

        player.Frozen = true;

        isDisplayingDialog = true;
        currentDialog = new Queue<string>(dialog);
        DialogPanel.SetActive(true);
        charsToDisplay = 0;
        ShowNextLine();
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

    //Close the dialog window and resume normal gameplay, or do the give item text and give the item
    private void EndDialog()
    {
        player.Frozen = false;
        isDisplayingDialog = false;
        DialogText.text = "";
        DialogPanel.SetActive(false);
        NewItemImage.SetActive(false);

        if (isGettingNewItem)
        {
            isGettingNewItem = false;
            DisplayItemGetUI(newItem);
            PlayerStats.Inventory.Add(newItem);
        }
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

    //Display an item image and the item's name and description.
    private void DisplayItemGetUI(Item item)
    {
        HandleDialogPress(new List<string>() { item.itemName + ": " + item.itemDescription });

        NewItemImage.GetComponent<Image>().sprite = item.itemSprite;
        NewItemImage.SetActive(true);
    }
}
