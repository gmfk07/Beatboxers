using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    public GameObject InventoryPanel;
    public GameObject InventorySlotPrefab;
    public GameObject InventorySlotParent;

    public int InventorySlotRows;
    public int InventorySlotColumns;
    public float MinInventorySlotX;
    public float MaxInventorySlotX;
    public float MinInventorySlotY;
    public float MaxInventorySlotY;

    private Dictionary<Vector2Int, GameObject> slotDict = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int selected = new Vector2Int(0, 0); //The top-right corner
    private Player player;
    private bool displayingInventory = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        CreateInventorySlots();
        CloseInventory();
    }

    private void Update()
    {
        HandleInventoryToggle();
        if (displayingInventory)
        {
            HandleSelection();
        }
    }

    //Listen to changes in horizontal and vertical axes to change the selected inventory slot
    private void HandleSelection()
    {
        if (Input.GetButtonDown("Left"))
        {
            MoveSelection(selected.x - 1, selected.y);
        }
        if (Input.GetButtonDown("Right"))
        {
            MoveSelection(selected.x + 1, selected.y);
        }
        if (Input.GetButtonDown("Down"))
        {
            MoveSelection(selected.x, selected.y - 1);
        }
        if (Input.GetButtonDown("Up"))
        {
            MoveSelection(selected.x, selected.y + 1);
        }
    }

    //Calls Select and Deselect on the relevant spots and moves selected to the new x and y, wrapping x and y around
    private void MoveSelection(int newX, int newY)
    {
        int lastColumn = InventorySlotColumns - 1;
        int lastRow = InventorySlotRows - 1;

        if (newX < 0)
            newX = lastColumn;
        else if (newX > lastColumn)
            newX = 0;

        if (newY < 0)
            newY = lastRow;
        else if (newY > lastRow)
            newY = 0;

        Vector2Int newSelected = new Vector2Int(newX, newY);
        DeselectSlot(selected);
        SelectSlot(newSelected);
        selected = newSelected;
    }

    //Listen for the inventory button being pressed and toggle the inventory.
    private void HandleInventoryToggle()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (displayingInventory)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    //Create the inventory slot GameObjects.
    private void CreateInventorySlots()
    {
        for (int i = 0; i < InventorySlotColumns; i++)
        {
            for (int j = 0; j < InventorySlotRows; j++)
            {
                float xPerColumn = (MaxInventorySlotX - MinInventorySlotX) / (InventorySlotColumns - 1) * i;
                float yPerRow = (MaxInventorySlotY - MinInventorySlotY) / (InventorySlotRows - 1) * j;
                float newX = MinInventorySlotX + xPerColumn;
                float newY = MinInventorySlotY + yPerRow;
                GameObject created = Instantiate(InventorySlotPrefab, new Vector3(newX, newY, 0), Quaternion.identity, InventorySlotParent.transform);
                slotDict[new Vector2Int(i, j)] = created;
            }
        }
    }

    //Turn on the inventory panel, select the right slot, and freeze the player
    void OpenInventory()
    {
        InventoryPanel.SetActive(true);
        displayingInventory = true;
        player.frozen = true;
        SelectSlot(selected);
    }

    //Turn off the inventory panel and reenable movement
    void CloseInventory()
    {
        InventoryPanel.SetActive(false);
        displayingInventory = false;
        player.frozen = false;
    }

    //Trigger the select method of a given slot to make it look selected
    private void SelectSlot(Vector2Int toSelect)
    {
        slotDict[toSelect].GetComponent<InventorySlot>().Select();
    }

    //Trigger the deselect method of a given slot to make it look deselected
    private void DeselectSlot(Vector2Int toDeselect)
    {
        slotDict[toDeselect].GetComponent<InventorySlot>().Deselect();
    }
}
