using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthGain;
    [SerializeField] private HealthDisplay healthDisplay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && PlayerStats.health != PlayerStats.maxHealth)
        {
            PlayerStats.Heal(healthGain, healthDisplay);
            Destroy(gameObject);
        }
    }
}
