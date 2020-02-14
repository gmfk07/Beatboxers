using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverworldEnemy : MonoBehaviour
{
    public Enemy enemy;
    private Animator battleStartAnim;
    private GameObject battleStartObject;
    private bool loadingLevel = false;

    //The highest-tier, true parent GameObject to this enemy. This parent should have the "EnemyParent" tag.
    [SerializeField] private GameObject enemyParent;

    private bool unloaded = false;

    private void Start()
    {
        battleStartObject = GameObject.FindGameObjectWithTag("BattleStart");
        battleStartAnim = battleStartObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (CanEnterBattle(collider))
        {
            //Start the battle enter animation and freeze the player
            loadingLevel = true;
            battleStartObject.GetComponent<Image>().enabled = true;
            battleStartAnim.SetTrigger("BattleStart");
            StartCoroutine("LoadBattleScene");
            collider.GetComponent<Player>().Frozen = true;
        }
    }

    //Returns true if the provided collider is the player, the overworld has fully loaded, this enemy is not being unloaded, and battle isn't starting with this enemy.
    private bool CanEnterBattle(Collider collider)
    {
        return collider.gameObject.tag == "Player" && OverworldManager.Instance.OverworldFullyLoaded && !unloaded & !loadingLevel;
    }

    //Destroys this game object and sets unloaded to true to ignore collisions until the object is properly destroyed.
    public void Unload()
    {
        unloaded = true;
        Destroy(enemyParent);
    }

    //Wait for victory animation to finish, then load
    IEnumerator LoadBattleScene()
    {
        EnemyStats.currentEnemy = enemy;
        yield return new WaitForSeconds(1.517f);
        OverworldManager.Instance.SaveGame(enemyParent);
        OverworldManager.Instance.GoToBattleScene();
    }
}
