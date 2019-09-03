﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class OverworldManager : Singleton<OverworldManager>
{
    public bool OverworldFullyLoaded;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    //Create an overworld save without the GameObject toRemove.
    private OverworldSave CreateOverworldSave(GameObject toRemove = null)
    {
        OverworldSave save = new OverworldSave();
        save.EnemyNameList = new List<string>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("EnemyParent"))
        {
            if (go != toRemove)
            {
                save.EnemyNameList.Add(go.name);
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
        if (SaveExists())
            File.Delete(Application.persistentDataPath + "/overworld.save");
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
                    Debug.Log(go.name + " vs " + name);

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
}
