using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    public int health;
    //The enemy will attack on all potential attack beats with potential values above or equal to attackMinimum
    public double attackMinimum;

    //The keys and values that'll go into attackDict
    public List<double> attackDictKeys = new List<double>();
    public List<EnemyAttack> attackDictValues = new List<EnemyAttack>();

    //Round beat potential down to the nearest key, the corresponding EnemyAttack is the EnemyAttack that will be executed
    public Dictionary<double, EnemyAttack> attackDict = new Dictionary<double, EnemyAttack>();

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
    public EnemyAttack GetAttack(double attackPotential)
    {
        double targetKey = -1;
        foreach (double d in attackDict.Keys)
        {
            //Do we have a key below the potential but above the targetKey?
            if (d <= attackPotential && d > targetKey)
                targetKey = d;
        }
        return attackDict[targetKey];
    }

    //Attack the player with a given EnemyAttack
    public void AttackPlayer(EnemyAttack attack)
    {
        PlayerStats.Damage(attack.damage);
    }
}
