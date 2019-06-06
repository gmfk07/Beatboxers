using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverworldSave
{
    public List<string> names = new List<string>();
    public List<float> enemyXPos = new List<float>();
    public List<float> enemyYPos = new List<float>();
    public List<float> enemyZPos = new List<float>();
    public float playerXPos;
    public float playerYPos;
    public float playerZPos;
}
