using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemy : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyStats.currentEnemy = enemy;
        //TODO: Save game
        SceneManager.LoadScene("Battle");
    }
}
