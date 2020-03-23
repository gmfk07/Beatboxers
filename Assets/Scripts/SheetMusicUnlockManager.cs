using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetMusicUnlockManager : Singleton<SheetMusicUnlockManager>
{
    //All costs must be different!
    [SerializeField] private int plantGrowCost;
    [SerializeField] private List<string> plantGrowDialog;
    [HideInInspector] public bool PlantGrowUnlocked;

    private bool inSheetMusicDialog;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        InitUnlocks();
    }

    private void Update()
    {
        //If we're in dialog, send interact button presses to DialogController.
        if (inSheetMusicDialog)
        {
            if (!player.Frozen)
            {
                inSheetMusicDialog = false;
            }
            else
            {
                if (Input.GetButtonDown("Interact"))
                {
                    DialogController.Instance.HandleDialogPress(null, null);
                }
            }
        }
    }

    //Silently unlock any abilities the player has the sheet music for.
    public void InitUnlocks()
    {
        PlantGrowUnlocked = PlayerStats.sheetMusic >= plantGrowCost;
    }

    //Handle reaching newSheetMusicCount, unlocking any necessary abilities and making dialog appear.
    public void CheckUnlocks(int newSheetMusicCount)
    {
        if (newSheetMusicCount == plantGrowCost)
        {
            PlantGrowUnlocked = true;
            DialogController.Instance.HandleDialogPress(plantGrowDialog, null);
            inSheetMusicDialog = true;
        }
    }
}
