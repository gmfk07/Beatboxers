using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] GameObject flagToCreate;
    [SerializeField] float timeUntilSceneChange;

    //If player collision, start next scene countdown and change material
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GetComponent<Renderer>().enabled = false;
            Instantiate(flagToCreate, transform);
            Invoke("GoToNextScene", timeUntilSceneChange);
        }
    }

    //Delete save and go to next scene
    private void GoToNextScene()
    {
        OverworldManager.Instance.DeleteSave();
        SceneManager.LoadScene(nextSceneName);
    }
}
