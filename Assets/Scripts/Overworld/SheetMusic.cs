using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetMusic : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerStats.sheetMusic++;
            Destroy(gameObject);
            SheetMusicUnlockManager.Instance.CheckUnlocks(PlayerStats.sheetMusic);
        }
    }
}
