using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class OverworldManager : Singleton<OverworldManager>
{
    public bool OverworldFullyLoaded;
    private AsyncOperation battleLoad;

    private void Start()
    {
        battleLoad = SceneManager.LoadSceneAsync("Battle");
        battleLoad.allowSceneActivation = false;
    }

    //Create an overworld save without the GameObject toRemove.
    private OverworldSave CreateOverworldSave(GameObject toRemove = null)
    {
        OverworldSave save = new OverworldSave();
        save.EnemyNameList = new List<string>();
        save.PickupNameList = new List<string>();
        save.NPCsSpokenTo = new List<string>();
        save.CutscenesPlayed = new List<string>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("EnemyParent"))
        {
            if (go != toRemove)
            {
                save.EnemyNameList.Add(go.name);
            }
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            save.PickupNameList.Add(go.name);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("NPC"))
        {
            if (go.GetComponent<NPC>().TalkedTo)
            {
                save.NPCsSpokenTo.Add(go.name);
            }
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Cutscene"))
        {
            if (go.GetComponent<Cutscene>().HasTriggered)
            {
                save.CutscenesPlayed.Add(go.name);
            }
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Questgiver"))
        {
            if (go.GetComponent<QuestgiverNPC>().HasTurnedInQuest)
            {
                save.QuestsCompleted.Add(go.name);
            }
        }

        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        save.PlayerXPos = playerPos.x;
        save.PlayerYPos = playerPos.y;
        save.PlayerZPos = playerPos.z;
        return save;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            SaveGame();
        if (Input.GetKeyDown(KeyCode.L))
            LoadGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        OverworldFullyLoaded = false;
        if (scene.name != "Battle")
        {
            if (SaveExists())
            {
                LoadGame();
            }
            OverworldFullyLoaded = true;
        }
    }

    //Returns true if a temporary overworld save exists, false otherwise.
    private static bool SaveExists()
    {
        return File.Exists(Application.persistentDataPath + "/overworld.save");
    }

    //Saves the overworld state without GameObject toRemove.
    public void SaveGame(GameObject toRemove = null)
    {
        OverworldSave save = CreateOverworldSave(toRemove);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/overworld.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    //Loads the saved game, instantiating only enemies from that save.
    public void LoadGame()
    {
        if (SaveExists())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/overworld.save", FileMode.Open);
            OverworldSave save = (OverworldSave)bf.Deserialize(file);
            file.Close();

            //Delete the enemies that aren't in the save
            //If it has the same name, we call it the same!!!
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("EnemyParent"))
            {
                bool isInSave = false;

                foreach (string name in save.EnemyNameList)
                {
                    if (go.name == name)
                    {
                        isInSave = true;
                        break;
                    }
                }

                if (!isInSave)
                {
                    go.GetComponentInChildren<OverworldEnemy>().Unload();
                }
            }

            //Delete the pickups that aren't in the save
            //If it has the same name, we call it the same!!!
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Pickup"))
            {
                bool isInSave = false;

                foreach (string name in save.PickupNameList)
                {
                    if (go.name == name)
                    {
                        isInSave = true;
                        break;
                    }
                }

                if (!isInSave)
                {
                    Destroy(go);
                }
            }

            //Make NPCs we've talked to still have the talked to variable checked
            //If it has the same name, we call it the same!!!
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("NPC"))
            {
                if (save.NPCsSpokenTo.Contains(go.name))
                {
                    go.GetComponent<NPC>().TalkedTo = true;
                    //Is this a Quest Task NPC? If so, mark the task as already completed.
                    if (go.GetComponent<QuestTaskNPC>() != null)
                    {
                        go.GetComponent<QuestTaskNPC>().CompleteTask();
                    }
                }
            }

            //Make cutscenes we've already triggered have the triggered variable checked
            //If it has the same name, we call it the same!!!
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Cutscene"))
            {
                if (save.CutscenesPlayed.Contains(go.name))
                {
                    go.GetComponent<Cutscene>().HasTriggered = true;
                }
            }

            //Make cutscenes we've already triggered have the triggered variable checked
            //If it has the same name, we call it the same!!!
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Questgiver"))
            {
                if (save.QuestsCompleted.Contains(go.name))
                {
                    go.GetComponent<QuestgiverNPC>().HasTurnedInQuest = true;
                    go.GetComponent<QuestgiverNPC>().GiveQuestRewards();
                }
            }

            Vector3 newPlayerPos = new Vector3(save.PlayerXPos, save.PlayerYPos, save.PlayerZPos);
            GameObject.FindGameObjectWithTag("Player").transform.position = newPlayerPos;

            Debug.Log("Game Loaded");
            File.Delete(Application.persistentDataPath + "/overworld.save");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    //If a temp save exists, delete it.
    public void DeleteSave()
    {
        if (SaveExists())
        {
            File.Delete(Application.persistentDataPath + "/overworld.save");
        }
    }

    //Go to the battle scene
    public void GoToBattleScene()
    {
        battleLoad.allowSceneActivation = true;
    }
}
