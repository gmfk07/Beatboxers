using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationController : Singleton<AttackAnimationController>
{
    [SerializeField] private Animator playerAttackAnimator;

    public void PlayPlayerAttackAnimation(string attackUsed)
    {
        playerAttackAnimator.SetTrigger(attackUsed);
    }
}
