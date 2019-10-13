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

    private static bool equipmentInitialized = false;

    public static List<Item> Inventory = new List<Item>();

    //Take the appropriate amount of damage, taking defending into account
    public static void Damage(int dmg)
    {
        if (isDefending)
        {
            dmg = Mathf.Max(dmg - currentDefense.ConstantProtection, 0);
            dmg -= Mathf.RoundToInt(dmg * (currentDefense.RelativeProtection));
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
            HandlePlayerDeath(overworldSaveExists);
        }
    }
    
    //Erase temp save, reload battle scene, refill HP
    private static void HandlePlayerDeath(bool overworldSaveExists)
    {
        if (overworldSaveExists)
        {
            File.Delete(Application.persistentDataPath + "/overworld.save");
        }

        health = 10;
        SceneManager.LoadScene("Test Overworld");
    }

    //Set the current defense value and note that we're defending
    public static void Defend(Defense def)
    {
        isDefending = true;
        currentDefense = def;
    }

    //If attacks and defenses haven't been initialized, sets the player's attacks and defenses to defaultAttack and defaultDefense and marks initialization
    public static void TryInitializeEquipment(Attack defaultAttack, Defense defaultDefense)
    {
        if (!equipmentInitialized)
        {
            upAttack = downAttack = leftAttack = rightAttack = defaultAttack;
            upDefense = downDefense = leftDefense = rightDefense = defaultDefense;
            equipmentInitialized = true;
        }
    }
}
