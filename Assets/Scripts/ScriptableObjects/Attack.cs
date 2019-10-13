using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Attack", order = 1)]
public class Attack : Item
{
    public int Damage;
    public Shape Shape;
}
