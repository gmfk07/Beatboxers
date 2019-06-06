using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class OverworldManager : Singleton<OverworldManager>
{
    private OverworldSave CreateOverworldSave()
    {
        OverworldSave save = new OverworldSave();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            OverworldEnemy oe = go.GetComponent<OverworldEnemy>();
            Vector3 pos = go.transform.position;
            save.names.Add(oe.enemy._name);
            save.xPos.Add(pos.x);
            save.yPos.Add(pos.y);
            save.zPos.Add(pos.z);
        }

        return save;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            SaveGame();
        if (Input.GetKeyDown(KeyCode.L))
            LoadGame();
    }

    private void OnApplicationQuit()
    {
        if (File.Exists(Application.persistentDataPath + "/overworld.save"))
            File.Delete(Application.persistentDataPath + "/overworld.save");
    }

    public void SaveGame()
    {
        OverworldSave save = CreateOverworldSave();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/overworld.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/overworld.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/overworld.save", FileMode.Open);
            OverworldSave save = (OverworldSave)bf.Deserialize(file);
            file.Close();

            //Delete all enemies
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(go);
            }

            //Reinstantiate the enemies
            for (int i=0; i < save.names.Count; i++)
            {
                GameObject go = OverworldEnemyDatabase.Instance.enemyDict[save.names[i]];
                float xPos = save.xPos[i];
                float yPos = save.yPos[i];
                float zPos = save.zPos[i];
                Instantiate(go, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            }

            Debug.Log("Game Loaded");
            File.Delete(Application.persistentDataPath + "/overworld.save");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}
