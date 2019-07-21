using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldEnemyDatabase : Singleton<OverworldEnemyDatabase>
{
    public List<string> nameKeys;
    public List<GameObject> prefabValues;
    public Dictionary<string, GameObject> enemyDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (FindObjectsOfType<OverworldEnemyDatabase>().Length > 1)
        {
            Destroy(gameObject);
        }

        for (int i=0; i < nameKeys.Count; i++)
        {
            enemyDict[nameKeys[i]] = prefabValues[i];
        }

        DontDestroyOnLoad(gameObject);
    }
}
