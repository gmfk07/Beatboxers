using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Defense", order = 1)]
public class Defense : ScriptableObject
{
    public int manaCost;
    public int constantProtection;
    //relative protection is a decimal fraction
    public float relativeProtection;
}
