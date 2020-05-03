using UnityEngine;

public class Window : Enemy
{

    private const string AttackAnimationTrigger = "Attack";

    public GameObject VasePrefab;

    protected override void OnAwake()
    {
        CheckDirection = false;
    }

    protected override void UpdateIdle()
    {
        if (IsPlayerOnAttackRange)
        {
            CurrentState = EEnemyState.Attack;
        }
    }

    protected override void UpdateAttack()
    {
        if (IsPlayerOnAttackRange)
        {
            if (CanAttack)
            {
                AttackCooldownTimer = AttackCooldown;
                Animator.SetTrigger(AttackAnimationTrigger);
            }

        }
        else
        {
            CurrentState = EEnemyState.Idle;

        }

    }

    public void ThrowVase()
    {
        Instantiate(VasePrefab, Head.transform.position, Quaternion.identity);

    }
}
