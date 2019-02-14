using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public float distancePerSecond = 0;
    public bool isAttackBeat;
    public EnemyAttack attack;

    private void Start()
    {
        if (isAttackBeat)
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1 - attack.danger, 1 - attack.danger, 1);
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
