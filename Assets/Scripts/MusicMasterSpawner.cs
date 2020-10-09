using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMasterSpawner : MonoBehaviour
{
    public GameObject ToSpawn;

    // Check if a MusicMaster exists. If not, spawn one.
    void Start()
    {
        if (MusicMaster.Instance == null)
            Instantiate(ToSpawn);
    }
}
