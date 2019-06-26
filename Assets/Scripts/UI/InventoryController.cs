using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    public List<Item> Inventory = new List<Item>();

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventorySlotParent;

    [SerializeField] private int inventorySlotRows;
    [SerializeField] private int inventorySlotColumns;
    [SerializeField] private float minInventorySlotX;
    [SerializeField] private float maxInventorySlotX;
    [SerializeField] private float minInventorySlotY;
    [SerializeField] private float maxInventorySlotY;

    private Dictionary<Vector2Int, InventorySlot> slotDict = new Dictionary<Vector2Int, InventorySlot>();
    private Vector2Int selected;
    private Player player;
    private bool displayingInventory = false;
    private bool equipping = false;
    private Item itemBeingEquipped;

    [SerializeField] private InventorySlot attackSlotUp;
    [SerializeField] private InventorySlot attackSlotLeft;
    [SerializeField] private InventorySlot attackSlotDown;
    [SerializeField] private InventorySlot attackSlotRight;

    [SerializeField] private InventorySlot defenseSlotUp;
    [SerializeField] private InventorySlot defenseSlotLeft;
    [SerializeField] private InventorySlot defenseSlotDown;
    [SerializeField] private InventorySlot defenseSlotRight;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        int topRow = inventorySlotRows - 1;
        selected = new Vector2Int(0, topRow);
        CreateInventorySlots();
        CloseInventory();
    }

    private void Update()
    {
        HandleInventoryToggle();
        if (displayingInventory)
        {
            if (!equipping)
            {
                HandleChangeSelected();
                if (Input.GetButtonDown("Interact"))
                {
                    equipping = true;
                    itemBeingEquipped = slotDict[selected].CurrentItem;
                    SelectEquipmentSlots();
                }
            }
            else
            {
                if (Input.GetButtonDown("Interact"))
                {
                    equipping = false;
                    DeselectEquipmentSlots();
                }
            }
        }
    }
    
    //Triggers the select effect on Attack or Defense equipment slots, based on itemBeingEquipped.
    private void SelectEquipmentSlots()
    {
        bool isAttack = itemBeingEquipped as Attack != null;
        bool isDefense = itemBeingEquipped as Defense != null;
        if (isAttack)
        {
            foreach (InventorySlot slot in new InventorySlot[4] { attackSlotUp, attackSlotDown, attackSlotLeft, attackSlotRight })
            {
                slot.Select();
            }
        }
        if (isDefense)
        {
            foreach (InventorySlot slot in new InventorySlot[4] { defenseSlotUp, defenseSlotDown, defenseSlotLeft, defenseSlotRight })
            {
                slot.Select();
            }
        }
    }

    //Triggers the deselect effect on all equipment slots.
    private void DeselectEquipmentSlots()
    {
        InventorySlot[] equipmentSlots = new InventorySlot[8] {attackSlotUp, attackSlotDown, attackSlotLeft, attackSlotRight,
            defenseSlotUp, defenseSlotDown, defenseSlotLeft, defenseSlotRight};

        foreach (InventorySlot slot in equipmentSlots)
        {
            slot.Deselect();
        }
    }

    //Listen to changes in horizontal and vertical axes to change the selected inventory slot
    private void HandleChangeSelected()
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
        int lastColumn = inventorySlotColumns - 1;
        int lastRow = inventorySlotRows - 1;

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

    //Create the inventory slot GameObjects and initialize using the inventory list.
    private void CreateInventorySlots()
    {
        for (int i = 0; i < inventorySlotColumns; i++)
        {
            for (int j = 0; j < inventorySlotRows; j++)
            {
                float xPerColumn = (maxInventorySlotX - minInventorySlotX) / (inventorySlotColumns - 1) * i;
                float yPerRow = (maxInventorySlotY - minInventorySlotY) / (inventorySlotRows - 1) * j;
                float newX = minInventorySlotX + xPerColumn;
                float newY = minInventorySlotY + yPerRow;
                GameObject created = Instantiate(inventorySlotPrefab, new Vector3(newX, newY, 0), Quaternion.identity, inventorySlotParent.transform);
                slotDict[new Vector2Int(i, j)] = created.GetComponent<InventorySlot>();
            }
        }
        SelectSlot(selected);
        UpdateSlotItems();
    }

    //Using the inventory list, adds all items in order into the appropriate slots
    private void UpdateSlotItems()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            int xPos = i % (inventorySlotColumns - 1);
            int yPos = inventorySlotRows - 1 - Mathf.FloorToInt(i / inventorySlotColumns);
            InventorySlot slot = slotDict[new Vector2Int(xPos, yPos)];
            slot.SetItem(Inventory[i]);
        }
    }

    //Turn on the inventory panel, select the right slot, and freeze the player
    void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        displayingInventory = true;
        player.frozen = true;
    }

    //Turn off the inventory panel and reenable movement
    void CloseInventory()
    {
        equipping = false;
        DeselectEquipmentSlots();
        inventoryPanel.SetActive(false);
        displayingInventory = false;
        player.frozen = false;
    }

    //Trigger the select method of a given slot to make it look selected
    private void SelectSlot(Vector2Int toSelect)
    {
        slotDict[toSelect].Select();
    }

    //Trigger the deselect method of a given slot to make it look deselected
    private void DeselectSlot(Vector2Int toDeselect)
    {
        slotDict[toDeselect].Deselect();
    }
}
