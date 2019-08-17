using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationController : Singleton<AttackAnimationController>
{
    [SerializeField] private Animator playerAttackAnimator;
    [SerializeField] private Animator enemyAttackAnimator;

    public void PlayPlayerAttackAnimation(string attackUsed)
    {
        playerAttackAnimator.SetTrigger(attackUsed);
    }

    public void PlayEnemyAttackAnimation(string attackUsed)
    {
        Debug.Log(enemyAttackAnimator);
        enemyAttackAnimator.SetTrigger(attackUsed);
    }
}
