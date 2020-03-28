using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartManager : Singleton<BattleStartManager>
{
    private Animator battleStartAnim;
    private GameObject battleStartObject;
    private Player player;
    [SerializeField] private float battleWaitTime;
    [HideInInspector] public bool LoadingLevel = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        battleStartObject = GameObject.FindGameObjectWithTag("BattleStart");
        battleStartAnim = battleStartObject.GetComponent<Animator>();
    }

    public void StartBattle(Enemy enemy, GameObject enemyParent = null)
    {
        //Start the battle enter animation and freeze the player
        LoadingLevel = true;
        battleStartObject.GetComponent<Image>().enabled = true;
        battleStartAnim.SetTrigger("BattleStart");
        StartCoroutine(LoadBattleScene(enemy, enemyParent));
        player.Frozen = true;
    }

    //Wait for victory animation to finish, then load
    IEnumerator LoadBattleScene(Enemy enemy, GameObject enemyParent)
    {
        EnemyStats.currentEnemy = enemy;
        yield return new WaitForSeconds(battleWaitTime);
        OverworldManager.Instance.SaveGame(enemyParent);
        OverworldManager.Instance.GoToBattleScene();
    }
}
