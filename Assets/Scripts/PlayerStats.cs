using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class PlayerStats
{
    public static string name1 = "Treble";

    public static Attack upAttack, downAttack, leftAttack, rightAttack;
    public static Defense upDefense, downDefense, leftDefense, rightDefense;
    public static int maxHealth = 10;
    public static int health = 10;
    public static int sheetMusic = 0;
    public static int lastLoadedScene = -1;

    private static bool isDefending = false;
    private static Defense currentDefense;

    private static bool equipmentInitialized = false;

    public static List<Item> Inventory = new List<Item>();
    public static Dictionary<string, int> QuestCounter = new Dictionary<string, int>();

    //Take the appropriate amount of damage, taking defending into account
    public static void Damage(int dmg, HealthDisplay hdToUpdate)
    {
        if (isDefending)
        {
            dmg = Mathf.Max(dmg - currentDefense.ConstantProtection, 0);
            dmg -= Mathf.RoundToInt(dmg * (currentDefense.RelativeProtection));
        }
        health = Mathf.Max(health - dmg, 0);

        hdToUpdate.UpdateHealthMeter();

        isDefending = false;

        bool overworldSaveExists = File.Exists(Application.persistentDataPath + "/overworld.save");

        if (health == 0)
        {
            HandlePlayerDeath(overworldSaveExists);
        }
    }

    //Heal the appropriate amount of health, taking maxHealth into account
    public static void Heal(int healthGain, HealthDisplay hdToUpdate)
    {
        health = Mathf.Min(health + healthGain, maxHealth);
        hdToUpdate.UpdateHealthMeter();
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

    //Returns true if a given quest key is recorded, false otherwise.
    public static bool CheckQuestStarted(string quest)
    {
        return QuestCounter.ContainsKey(quest);
    }

    //Initiates a given quest with counter 0.
    public static void StartQuest(string quest)
    {
        QuestCounter[quest] = 0;
    }

    //Add one to a given quest counter.
    public static void IncrementQuestCounter(string quest)
    {
        QuestCounter[quest]++;
    }
}
