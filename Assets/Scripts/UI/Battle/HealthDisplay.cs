using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : Singleton<HealthDisplay>
{
    [SerializeField] private List<Sprite> healthMeterSprites;
    [SerializeField] private Image healthMeterImage;

    public void UpdateHealthMeter()
    {
        healthMeterImage.sprite = healthMeterSprites[PlayerStats.health];
    }
}
