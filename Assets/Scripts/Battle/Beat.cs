using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public float DistancePerSecond = 0;
    public bool IsAttackBeat;
    public EnemyAttack Attack;
    public Shape Shape;
    public float Redness;
    public HealthDisplay HealthDisplay;

    private float levelLoadTimeEpsilon = 0.25f; //Don't deal damage if the last level was loaded at most epsilon seconds ago

    private void Start()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (IsAttackBeat)
        {
            sr.color = new Color(1, 1 - Redness, 1 - Redness, 1);
        }
        switch (Shape)
        {
            case Shape.Circle:
                sr.sprite = GlobalStats.circleBeat;
                break;

            case Shape.Square:
                sr.sprite = GlobalStats.squareBeat;
                break;

            case Shape.Triangle:
                sr.sprite = GlobalStats.triangleBeat;
                break;

            case Shape.Diamond:
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
        //Attack the player IF this beat was not destroyed as a result of a scene change
        if (IsAttackBeat && Time.timeSinceLevelLoad > levelLoadTimeEpsilon)
        {
            Debug.Log("bro you just got attacked");
            PlayerStats.Damage(Attack.Damage, HealthDisplay);
            AttackAnimationController.Instance.PlayEnemyAttackAnimation(Attack.AttackName);
        }
    }
}
