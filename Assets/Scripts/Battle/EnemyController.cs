using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    
    public int health;
    //The enemy will attack on all potential attack beats with potential values above or equal to attackMinimum
    public float attackMinimum;

    //The keys and values that'll go into attackDict
    public List<float> attackDictKeys = new List<float>();
    public List<EnemyAttack> attackDictValues = new List<EnemyAttack>();

    //Round beat potential down to the nearest key, the corresponding EnemyAttack is the EnemyAttack that will be executed
    public Dictionary<float, EnemyAttack> attackDict = new Dictionary<float, EnemyAttack>();

    public void Start()
    {
        //Set up the actual dictionary
        for (int i = 0; i < attackDictKeys.Count; i++)
            attackDict.Add(attackDictKeys[i], attackDictValues[i]);
    }

    public void Hit(Attack attack)
    {
        int damage = attack.damage;
        health = Mathf.Max(health - damage, 0);
        if (health == 0)
        {
            Destroy(gameObject);
            //TODO: The battle is won
        }
    }

    //Get the attack associated with a given attackPotential, given that the attackPotential is greater than attackMinimum
    public EnemyAttack GetAttack(float attackPotential)
    {
        float targetKey = -1;
        foreach (float a in attackDict.Keys)
        {
            //Do we have a key below the potential but above the targetKey?
            if (a <= attackPotential && a > targetKey)
                targetKey = a;
        }
        return attackDict[targetKey];
    }
}
