﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour {
    
    public int health;
    //The enemy will attack on all potential attack beats with potential values above or equal to attackMinimum
    public float AttackMinimum;

    //Round beat potential down to the nearest key, the corresponding EnemyAttack is the EnemyAttack that will be executed
    private Dictionary<float, EnemyAttack> attackDict = new Dictionary<float, EnemyAttack>();

    public void Start()
    {
        Enemy enemy = EnemyStats.currentEnemy;

        AttackMinimum = Mathf.Min(enemy.attackDictKeys.ToArray());

        //Set up the actual dictionary
        for (int i = 0; i < enemy.attackDictKeys.Count; i++)
        {
            attackDict.Add(enemy.attackDictKeys[i], enemy.attackDictValues[i]);
        }
    }

    public void Hit(Attack attack)
    {
        int damage = attack.damage;
        health = Mathf.Max(health - damage, 0);
        if (health == 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(1);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SceneManager.LoadScene(1);
    }
}
