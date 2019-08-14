using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public float distancePerSecond = 0;
    public bool isAttackBeat;
    public EnemyAttack attack;
    public GlobalStats.Shape shape;

    private void Start()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (isAttackBeat)
            sr.color = new Color(1, 1 - attack.Danger, 1 - attack.Danger, 1);
        switch (shape)
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
        float movement = distancePerSecond * Time.deltaTime;
        gameObject.transform.position += new Vector3(-movement, 0);
    }

    void OnDestroy()
    {
        //Attack the player
        if (isAttackBeat)
        {
            PlayerStats.Damage(attack.Damage);
            AttackAnimationController.Instance.PlayEnemyAttackAnimation(attack.AttackName);
        }
    }
}
