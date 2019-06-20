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
                Instantiate(InventorySlotPrefab, new Vector3(newX, newY, 0), Quaternion.identity, InventorySlotParent.transform);
            }
        }
    }

    //Turn on the inventory panel and freeze the player
    void OpenInventory()
    {
        InventoryPanel.SetActive(true);
        displayingInventory = true;
        player.frozen = true;
    }

    //Turn off the inventory panel and reenable movement
    void CloseInventory()
    {
        InventoryPanel.SetActive(false);
        displayingInventory = false;
        player.frozen = false;
    }
}
