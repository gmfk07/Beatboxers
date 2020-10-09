using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public string _name;
    public int health;

    //Round beat potential down to the nearest key, the corresponding EnemyAttack is the EnemyAttack that will be executed
    public Dictionary<float, EnemyAttack> attackDict = new Dictionary<float, EnemyAttack>();

    //The keys and values that'll go into attackDict
    public List<float> attackDictKeys = new List<float>();
    public List<EnemyAttack> attackDictValues = new List<EnemyAttack>();
}
