using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : Singleton<InventoryUIController>
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventorySlotParent;
    [SerializeField] private Text itemDescriptionText;

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

    public List<Item> tempInventory = new List<Item>(); //DELETE LATER!

    private void Start()
    {
        PlayerStats.Inventory = tempInventory;
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
                bool slotHasItem = slotDict[selected].CurrentItem != null;
                if (Input.GetButtonDown("Interact") && slotHasItem)
                {
                    equipping = true;
                    itemBeingEquipped = slotDict[selected].CurrentItem;
                    SelectEquipmentSlots();
                }
            }
            else
            {
                bool interactPressed = Input.GetButtonDown("Interact");
                bool leftPressed = Input.GetButtonDown("Left");
                bool rightPressed = Input.GetButtonDown("Right");
                bool upPressed = Input.GetButtonDown("Up");
                bool downPressed = Input.GetButtonDown("Down");
                bool anythingPressed = interactPressed || leftPressed || rightPressed || upPressed || downPressed;
                if (anythingPressed)
                {
                    bool selectedItemIsAttack = slotDict[selected].CurrentItem as Attack != null;
                    bool selectedItemIsDefense = !selectedItemIsAttack;
                    if (selectedItemIsAttack)
                    {
                        Attack attackEquipped = itemBeingEquipped as Attack;
                        if (leftPressed)
                        {
                            attackSlotLeft.SetItem(itemBeingEquipped);
                            PlayerStats.leftAttack = attackEquipped;
                        }
                        if (rightPressed)
                        {
                            attackSlotRight.SetItem(itemBeingEquipped);
                            PlayerStats.rightAttack = attackEquipped;
                        }
                        if (upPressed)
                        {
                            attackSlotUp.SetItem(itemBeingEquipped);
                            PlayerStats.upAttack = attackEquipped;
                        }
                        if (downPressed)
                        {
                            attackSlotDown.SetItem(itemBeingEquipped);
                            PlayerStats.leftAttack = attackEquipped;
                        }
                    }
                    else if (selectedItemIsDefense)
                    {
                        Defense defenseEquipped = itemBeingEquipped as Defense;
                        if (leftPressed)
                        {
                            defenseSlotLeft.SetItem(itemBeingEquipped);
                            PlayerStats.leftDefense = defenseEquipped;
                        }
                        if (rightPressed)
                        {
                            defenseSlotRight.SetItem(itemBeingEquipped);
                            PlayerStats.rightDefense = defenseEquipped;
                        }
                        if (upPressed)
                        {
                            defenseSlotUp.SetItem(itemBeingEquipped);
                            PlayerStats.upDefense = defenseEquipped;
                        }
                        if (downPressed)
                        {
                            defenseSlotDown.SetItem(itemBeingEquipped);
                            PlayerStats.downDefense = defenseEquipped;
                        }
                    }
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

    //Calls Select and Deselect on the relevant spots and moves selected to the new x and y, wrapping x and y around, and finally updates the description
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

        UpdateDescription();
    }

    //Listen for the inventory button being pressed and toggle the inventory, updating the description if the inventory is being opened.
    private void HandleInventoryToggle()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (displayingInventory)
            {
                CloseInventory();
            }
            else if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().frozen)
            {
                OpenInventory();
                UpdateDescription();
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
        for (int i = 0; i < PlayerStats.Inventory.Count; i++)
        {
            int xPos = i % (inventorySlotColumns - 1);
            int yPos = inventorySlotRows - 1 - Mathf.FloorToInt(i / inventorySlotColumns);
            InventorySlot slot = slotDict[new Vector2Int(xPos, yPos)];
            slot.SetItem(PlayerStats.Inventory[i]);
        }
    }

    //Update the UI Description Text to display the currently selected item's description, or the empty string if nothing is selected
    private void UpdateDescription()
    {
        if (slotDict.ContainsKey(selected))
        {
            itemDescriptionText.text = slotDict[selected].CurrentItem.itemDescription;
        }
        else
        {
            itemDescriptionText.text = "";
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
