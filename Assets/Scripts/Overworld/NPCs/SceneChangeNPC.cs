using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeNPC : NPC
{
    [SerializeField] string nextSceneName;

    public override void HandleDialogBegin()
    {
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Frozen)
        {
            SceneManager.LoadScene(nextSceneName);
            OverworldManager.Instance.SaveGame();
        }
    }
}
