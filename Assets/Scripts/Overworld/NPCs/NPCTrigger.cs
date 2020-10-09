using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    private NPC parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.GetComponentInParent<NPC>();
    }

    //Check for player getting close
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().NPCNearbyList.Add(parent);
        }
    }

    //Check for player leaving
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().NPCNearbyList.Remove(parent);
        }
    }
}
