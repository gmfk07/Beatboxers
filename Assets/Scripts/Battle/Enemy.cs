using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public string _name;
    public int health;

    //The enemy will attack on all potential attack beats with potential values above or equal to attackMinimum
    public float attackMinimum;

    //Round beat potential down to the nearest key, the corresponding EnemyAttack is the EnemyAttack that will be executed
    public Dictionary<float, EnemyAttack> attackDict = new Dictionary<float, EnemyAttack>();

}
