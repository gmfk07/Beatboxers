using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Defense", order = 1)]
public class Defense : Item
{
    public int ConstantProtection;
    //relative protection is a decimal fraction
    public float RelativeProtection;
    public Shape Shape;
}
