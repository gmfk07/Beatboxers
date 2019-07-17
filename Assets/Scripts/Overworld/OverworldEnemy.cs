using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemy : MonoBehaviour
{
    public Enemy enemy;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.tag == "Player" && OverworldManager.Instance.OverworldFullyLoaded)
        {
            EnemyStats.currentEnemy = enemy;
            OverworldManager.Instance.SaveGame(gameObject);
            SceneManager.LoadScene("Battle");
        }
    }
}
