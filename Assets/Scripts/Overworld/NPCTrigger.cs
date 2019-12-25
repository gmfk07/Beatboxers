using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    private NPC parent;
    private bool playerNear = false;
    public bool TalkedTo = false;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.GetComponentInParent<NPC>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && playerNear)
        {
            if (parent.HasRepeatDialog && TalkedTo)
            {
                DialogController.Instance.HandleDialogPress(parent.RepeatDialog, null);
            }
            else
            {
                DialogController.Instance.HandleDialogPress(parent.Dialog, parent.ItemToGive);
                TalkedTo = true;
            }
        }
    }

    //Check for player getting close
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            playerNear = true;
    }

    //Check for player leaving
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            playerNear = false;
    }
}
