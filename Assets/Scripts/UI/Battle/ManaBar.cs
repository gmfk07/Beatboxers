using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private Image manaBarImage;

    //Updates the bar to show that the player has the given amount of mana
    public void UpdateManaBar(int mana)
    {
        manaBarImage.sprite = sprites[mana];
    }
}
