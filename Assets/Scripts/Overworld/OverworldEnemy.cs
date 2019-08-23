using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemy : MonoBehaviour
{
    public Enemy enemy;

    //The highest-tier, true parent GameObject to this enemy. This parent should have the "EnemyParent" tag.
    [SerializeField] private GameObject enemyParent;

    private bool unloaded = false;

    private void OnTriggerEnter(Collider collider)
    {
        
        if (CanEnterBattle(collider))
        {
            EnterBattle();
        }
    }

    //Returns true if the provided collider is the player, the overworld has fully loaded, and this enemy is not being unloaded.
    private bool CanEnterBattle(Collider collider)
    {
        return collider.gameObject.tag == "Player" && OverworldManager.Instance.OverworldFullyLoaded && !unloaded;
    }

    //Enter battle against this enemy and remove it from the overworld.
    private void EnterBattle()
    {
        EnemyStats.currentEnemy = enemy;
        OverworldManager.Instance.SaveGame(enemyParent);
        SceneManager.LoadScene("Battle");
    }

    //Destroys this game object and sets unloaded to true to ignore collisions until the object is properly destroyed.
    public void Unload()
    {
        unloaded = true;
        Destroy(enemyParent);
    }
}
