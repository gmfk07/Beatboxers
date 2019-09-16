using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyAttack", order = 1)]
public class EnemyAttack : ScriptableObject
{
    public int Damage;
    public string AttackName;
    public float Redness; //How red beats with this enemy attack should be, with 0 being totally white and 1 being totally red
}
