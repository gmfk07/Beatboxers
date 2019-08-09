using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class PlayerStats
{
    public static Attack upAttack, downAttack, leftAttack, rightAttack;
    public static Defense upDefense, downDefense, leftDefense, rightDefense;
    public static int health = 10;
    private static bool isDefending = false;
    private static Defense currentDefense;

    public static List<Item> Inventory = new List<Item>();

    //Take the appropriate amount of damage, taking defending into account
    public static void Damage(int dmg)
    {
        if (isDefending)
        {
            dmg -= currentDefense.constantProtection;
            dmg -= Mathf.RoundToInt(dmg * (currentDefense.relativeProtection));
        }
        health = Mathf.Max(health - dmg, 0);

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Battle"))
        {
            HealthDisplay.Instance.UpdateHealthMeter();
        }

        isDefending = false;

        bool overworldSaveExists = File.Exists(Application.persistentDataPath + "/overworld.save");
        if (health == 0)
        {
            if (overworldSaveExists)
            {
                File.Delete(Application.persistentDataPath + "/overworld.save");
            }
            SceneManager.LoadScene("Test Overworld");
        }
    }

    //Set the current defense value and note that we're defending
    public static void Defend(Defense def)
    {
        isDefending = true;
        currentDefense = def;
    }

    //Set the player's attacks and defenses to defaultAttack and defaultDefense
    public static void InitializeEquipment(Attack defaultAttack, Defense defaultDefense)
    {
        upAttack = downAttack = leftAttack = rightAttack = defaultAttack;
        upDefense = downDefense = leftDefense = rightDefense = defaultDefense;
    }
}
