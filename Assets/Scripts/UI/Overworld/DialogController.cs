using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : Singleton<DialogController>
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject newItemImage;
    [SerializeField] private Text dialogText;
    [SerializeField] private float timeBetweenCharacters;
    [SerializeField] private Color defaultTextColor;
    [SerializeField] private List<string> colorCodeKeys;
    [SerializeField] private List<Color> colorCodeValues;

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
        dialogPanel.SetActive(false);
        dialogText.text = "";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        newItemImage.SetActive(false);
    }

    //Handle dialog button being pressed in response to a dialog with dialog dialog and giving item toGet.
    //If no dialog is currently being displayed, this starts the dialog and sets up the item to receive at the end. Otherwise, this merely continues it.
    public void HandleDialogPress(List<string> dialog, Item toGet = null)
    {
        //Leave if player is in inventory
        if (!isDisplayingDialog && player.Frozen)
            return;

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
        dialogPanel.SetActive(true);
        charsToDisplay = 0;
        ShowNextLine();
    }

    //Handle displaying the line of dialog atop the queue, or ending if the queue is empty
    private void ShowNextLine()
    {
        StopAllCoroutines();
        charsToDisplay = 0;
        isRevealingChars = true;

        if (currentDialog.Count > 0)
        {
            stringToDisplay = currentDialog.Dequeue();

            //Update color based on color code
            string code = UpdateTextColor(stringToDisplay);
            //Remove the color code from the string
            if (code != "none")
            {
                stringToDisplay = stringToDisplay.Substring(code.Length);
            }

            StartCoroutine(IncreaseCharsToDisplay());
        }
        else {
            EndDialog();
        }
    }

    //Given the dialog that will be displayed, sets the text color based on the color code at the start of the dialog and returns the color code, or "none" if none was found.
    private string UpdateTextColor(string toDisplay)
    {
        dialogText.color = defaultTextColor;
        string ret = "none";

        for (int i = 0; i < colorCodeKeys.Count; i++)
        {
            string code = colorCodeKeys[i];
            if (toDisplay.StartsWith(code))
            {
                ret = code;
                dialogText.color = colorCodeValues[i];
            }
        }

        return ret;
    }

    //Close the dialog window and resume normal gameplay, or do the give item text and give the item
    private void EndDialog()
    {
        player.Frozen = false;
        isDisplayingDialog = false;
        dialogText.text = "";
        dialogPanel.SetActive(false);
        newItemImage.SetActive(false);

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
        dialogText.text = stringToDisplay.Substring(0, stringToDisplay.Length);
        isRevealingChars = false;
        StopAllCoroutines();
    }

    //Display one more character
    private IEnumerator IncreaseCharsToDisplay()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenCharacters);
            if (charsToDisplay < stringToDisplay.Length)
            {
                charsToDisplay++;
            }
            else
            {
                isRevealingChars = false;
            }
            dialogText.text = stringToDisplay.Substring(0, charsToDisplay);
        }
    }

    //Display an item image and the item's name and description.
    private void DisplayItemGetUI(Item item)
    {
        HandleDialogPress(new List<string>() { item.itemName + ": " + item.itemDescription });

        newItemImage.GetComponent<Image>().sprite = item.itemSprite;
        newItemImage.SetActive(true);
    }
}
