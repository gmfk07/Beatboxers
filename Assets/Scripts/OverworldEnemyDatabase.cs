using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldEnemyDatabase : Singleton<OverworldEnemyDatabase>
{
    public List<GameObject> enemyPrefabs;

    //Maps names to enemy prefabs
    public Dictionary<string, GameObject> enemyDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (FindObjectsOfType<OverworldEnemyDatabase>().Length > 1)
        {
            Destroy(gameObject);
        }

        for (int i=0; i < enemyPrefabs.Count; i++)
        {
            enemyDict[enemyPrefabs[i].name] = enemyPrefabs[i];
        }

        DontDestroyOnLoad(gameObject);
    }
}
