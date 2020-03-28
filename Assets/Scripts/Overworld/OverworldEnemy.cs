using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            BattleStartManager.Instance.StartBattle(enemy, enemyParent);
        }
    }

    //Returns true if the provided collider is the player, the overworld has fully loaded, this enemy is not being unloaded, and battle isn't starting with this enemy.
    private bool CanEnterBattle(Collider collider)
    {
        return collider.gameObject.tag == "Player" && OverworldManager.Instance.OverworldFullyLoaded && !unloaded & !BattleStartManager.Instance.LoadingLevel;
    }

    //Destroys this game object and sets unloaded to true to ignore collisions until the object is properly destroyed.
    public void Unload()
    {
        unloaded = true;
        Destroy(enemyParent);
    }
}
