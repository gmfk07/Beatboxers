using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5;

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
}
