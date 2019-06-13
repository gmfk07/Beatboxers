using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private List<string> dialog = new List<string>();
    private bool speaking = false;

    public void ShowDialog()
    {
        if (!speaking)
        {
            speaking = true;
            Debug.Log("blah blah");
            //Tell DialogManager
        }
    }
}
