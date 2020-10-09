using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private List<Sprite> healthMeterSprites;
    [SerializeField] private Image healthMeterImage;

    private void Start()
    {
        UpdateHealthMeter();
    }

    //Updates the health display
    public void UpdateHealthMeter()
    {
        healthMeterImage.sprite = healthMeterSprites[PlayerStats.health];
    }
}
