using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public float DistancePerSecond = 0;
    public bool IsAttackBeat;
    public EnemyAttack Attack;
    public GlobalStats.Shape Shape;
    public float Danger;

    private void Start()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (IsAttackBeat)
        {
            sr.color = new Color(1, 1 - Danger, 1 - Danger, 1);
        }
        switch (Shape)
        {
            case GlobalStats.Shape.CIRCLE:
                sr.sprite = GlobalStats.circleBeat;
                break;

            case GlobalStats.Shape.SQUARE:
                sr.sprite = GlobalStats.squareBeat;
                break;

            case GlobalStats.Shape.TRIANGLE:
                sr.sprite = GlobalStats.triangleBeat;
                break;

            case GlobalStats.Shape.DIAMOND:
                sr.sprite = GlobalStats.diamondBeat;
                break;
        }
    }

    void Update()
    {
        float movement = DistancePerSecond * Time.deltaTime;
        gameObject.transform.position += new Vector3(-movement, 0);
    }

    void OnDestroy()
    {
        //Attack the player
        if (IsAttackBeat)
        {
            PlayerStats.Damage(Attack.Damage);
            AttackAnimationController.Instance.PlayEnemyAttackAnimation(Attack.AttackName);
        }
    }
}
