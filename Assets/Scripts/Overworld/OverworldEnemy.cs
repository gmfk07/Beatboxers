using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemy : MonoBehaviour
{
    public Enemy enemy;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.tag == "Player")
        {
            EnemyStats.currentEnemy = enemy;
            Destroy(this);
            OverworldManager.Instance.SaveGame();
            SceneManager.LoadScene("Battle");
        }
    }
}
