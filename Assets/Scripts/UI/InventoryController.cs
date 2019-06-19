using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    public GameObject InventoryPanel;

    private Player player;
    private bool displayingInventory = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
