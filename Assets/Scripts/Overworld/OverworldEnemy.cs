using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemy : MonoBehaviour
{
    public Enemy enemy;

    private bool unloaded = false;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        if (collider.gameObject.tag == "Player" && OverworldManager.Instance.OverworldFullyLoaded && !unloaded)
        {
            EnemyStats.currentEnemy = enemy;
            OverworldManager.Instance.SaveGame(gameObject);
            SceneManager.LoadScene("Battle");
        }
    }

    public void Unload()
    {
        unloaded = true;
        Destroy(gameObject);
    }
}
