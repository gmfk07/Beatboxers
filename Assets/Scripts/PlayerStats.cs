using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static Attack upAttack, downAttack, leftAttack, rightAttack;
    public static int health = 100;

    public static void Damage(int dmg)
    {
        health = Mathf.Max(health - dmg, 0);
        //TODO: Do something if no health?
    }
}
