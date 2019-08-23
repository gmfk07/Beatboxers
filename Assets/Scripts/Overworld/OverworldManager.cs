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

    //Create an overworld save without the GameObject toRemove.
    private OverworldSave CreateOverworldSave(GameObject toRemove = null)
    {
        OverworldSave save = new OverworldSave();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("EnemyParent"))
        {
            if (go != toRemove)
            {
                OverworldEnemy oe = go.GetComponentInChildren<OverworldEnemy>();
                Vector3 pos = go.transform.position;
                Quaternion rot = go.transform.rotation;

                save.names.Add(oe.enemy._name);
                save.enemyXPos.Add(pos.x);
                save.enemyYPos.Add(pos.y);
                save.enemyZPos.Add(pos.z);
                save.enemyXRot.Add(rot.x);
                save.enemyYRot.Add(rot.y);
                save.enemyZRot.Add(rot.z);
                save.enemyWRot.Add(rot.w);
            }
        }
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        save.playerXPos = playerPos.x;
        save.playerYPos = playerPos.y;
        save.playerZPos = playerPos.z;
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

            //Delete all enemies
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("EnemyParent"))
            {
                go.GetComponentInChildren<OverworldEnemy>().Unload();
            }

            //Reinstantiate the enemies
            for (int i=0; i < save.names.Count; i++)
            {
                GameObject go = OverworldEnemyDatabase.Instance.enemyDict[save.names[i]];
                float xPos = save.enemyXPos[i];
                float yPos = save.enemyYPos[i];
                float zPos = save.enemyZPos[i];
                float xRot = save.enemyXRot[i];
                float yRot = save.enemyYRot[i];
                float zRot = save.enemyZRot[i];
                float wRot = save.enemyWRot[i];
                Instantiate(go, new Vector3(xPos, yPos, zPos), new Quaternion(xRot, yRot, zRot, wRot));
            }
            Vector3 newPos = new Vector3(save.playerXPos, save.playerYPos, save.playerZPos);
            GameObject.FindGameObjectWithTag("Player").transform.position = newPos;

            Debug.Log("Game Loaded");
            File.Delete(Application.persistentDataPath + "/overworld.save");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}
