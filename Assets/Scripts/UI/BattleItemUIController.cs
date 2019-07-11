using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemUIController : MonoBehaviour
{
    [SerializeField] private GameObject UpAttackImage;
    [SerializeField] private GameObject LeftAttackImage;
    [SerializeField] private GameObject DownAttackImage;
    [SerializeField] private GameObject RightAttackImage;

    [SerializeField] private GameObject UpDefenseImage;
    [SerializeField] private GameObject LeftDefenseImage;
    [SerializeField] private GameObject DownDefenseImage;
    [SerializeField] private GameObject RightDefenseImage;

    private void Start()
    {
        UpAttackImage.GetComponent<Image>().sprite = PlayerStats.upAttack.itemSprite;
        LeftAttackImage.GetComponent<Image>().sprite = PlayerStats.leftAttack.itemSprite;
        DownAttackImage.GetComponent<Image>().sprite = PlayerStats.downAttack.itemSprite;
        RightAttackImage.GetComponent<Image>().sprite = PlayerStats.rightAttack.itemSprite;

        UpDefenseImage.GetComponent<Image>().sprite = PlayerStats.upDefense.itemSprite;
        LeftDefenseImage.GetComponent<Image>().sprite = PlayerStats.leftDefense.itemSprite;
        DownDefenseImage.GetComponent<Image>().sprite = PlayerStats.downDefense.itemSprite;
        RightDefenseImage.GetComponent<Image>().sprite = PlayerStats.rightDefense.itemSprite;

        UpAttackImage.GetComponentInChildren<Text>().text = PlayerStats.upAttack.manaCost.ToString();
        LeftAttackImage.GetComponentInChildren<Text>().text = PlayerStats.leftAttack.manaCost.ToString();
        DownAttackImage.GetComponentInChildren<Text>().text = PlayerStats.downAttack.manaCost.ToString();
        RightAttackImage.GetComponentInChildren<Text>().text = PlayerStats.rightAttack.manaCost.ToString();

        UpDefenseImage.GetComponentInChildren<Text>().text = PlayerStats.upDefense.manaCost.ToString();
        LeftDefenseImage.GetComponentInChildren<Text>().text = PlayerStats.leftDefense.manaCost.ToString();
        DownDefenseImage.GetComponentInChildren<Text>().text = PlayerStats.downDefense.manaCost.ToString();
        RightDefenseImage.GetComponentInChildren<Text>().text = PlayerStats.rightDefense.manaCost.ToString();
    }
}
