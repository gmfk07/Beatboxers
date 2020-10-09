using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverworldSave
{
    public List<string> PickupNameList;
    public List<string> EnemyNameList;
    public List<string> NPCsSpokenTo;
    public List<string> CutscenesPlayed;
    public List<string> QuestsCompleted;
    public float PlayerXPos;
    public float PlayerYPos;
    public float PlayerZPos;
}
