using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static Attack upAttack, downAttack, leftAttack, rightAttack;
    public static Defense upDefense, downDefense, leftDefense, rightDefense;
    public static int health = 10;
    private static bool isDefending = false;
    private static Defense currentDefense;

    public static void Damage(int dmg)
    {
        if (isDefending)
        {
            dmg -= currentDefense.constantProtection;
            dmg -= Mathf.RoundToInt(dmg * (currentDefense.relativeProtection));
        }
        health = Mathf.Max(health - dmg, 0);
        isDefending = false;
        //TODO: Do something if no health?
    }

    public static void Defend(Defense def)
    {
        isDefending = true;
        currentDefense = def;
    }
}
