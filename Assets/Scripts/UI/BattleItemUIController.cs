using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemUIController : MonoBehaviour
{
    [SerializeField] private Image UpAttackImage;
    [SerializeField] private Image LeftAttackImage;
    [SerializeField] private Image DownAttackImage;
    [SerializeField] private Image RightAttackImage;

    [SerializeField] private Image UpDefenseImage;
    [SerializeField] private Image LeftDefenseImage;
    [SerializeField] private Image DownDefenseImage;
    [SerializeField] private Image RightDefenseImage;

    private void Start()
    {
        UpAttackImage.sprite = PlayerStats.upAttack.itemSprite;
        LeftAttackImage.sprite = PlayerStats.leftAttack.itemSprite;
        DownAttackImage.sprite = PlayerStats.downAttack.itemSprite;
        RightAttackImage.sprite = PlayerStats.rightAttack.itemSprite;

        UpDefenseImage.sprite = PlayerStats.upDefense.itemSprite;
        LeftDefenseImage.sprite = PlayerStats.leftDefense.itemSprite;
        DownDefenseImage.sprite = PlayerStats.downDefense.itemSprite;
        RightDefenseImage.sprite = PlayerStats.rightDefense.itemSprite;
    }
}
