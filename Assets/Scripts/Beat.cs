using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public enum Shape { CIRCLE, SQUARE };

    public float distancePerSecond = 0;
    public bool isAttackBeat;
    public EnemyAttack attack;
    public Shape shape;

    private void Start()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (isAttackBeat)
            sr.color = new Color(1, 1 - attack.danger, 1 - attack.danger, 1);
        switch (shape)
        {
            case Shape.CIRCLE:
                sr.sprite = GlobalStats.circleBeat;
                break;

            case Shape.SQUARE:
                sr.sprite = GlobalStats.squareBeat;
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
            PlayerStats.Damage(attack.damage);
    }
}
