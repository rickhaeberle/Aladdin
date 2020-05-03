using UnityEngine;

public class Zagoolien : Enemy
{

    private const string IdleAnimationTrigger = "Idle";
    private const string ProvokeAnimationTrigger = "Provoke";
    private const string WalkAnimationTrigger = "Walk";
    private const string AttackAnimationTrigger = "Attack";

    public GameObject KnifePrefab;

    private EnemyHurtbox _enemyHurtbox;

    private void Start()
    {
        _enemyHurtbox = GetComponentInChildren<EnemyHurtbox>();
    }

    protected override void UpdateIdle()
    {

        if (IsPlayerOnFollowRange)
        {
            if (CanWalk)
            {
                CurrentState = EEnemyState.Follow;

            }
        }
        else if (IsPlayerOnSightRange)
        {
            Animator.SetTrigger(ProvokeAnimationTrigger);

        }
        else
        {
            Animator.SetTrigger(IdleAnimationTrigger);

        }

    }

    protected override void UpdateFollow()
    {

        if (IsPlayerOnAttackRange)
        {
            CurrentState = EEnemyState.Attack;
        }
        else if (!IsPlayerOnSightRange)
        {
            TeleportToStartPosition();
        }
        else if (CanWalk)
        {
            if(!IsTakingDamage) {
                Animator.SetTrigger(WalkAnimationTrigger);
                Rigidbody2D.velocity = new Vector2(MoveSpeed * Direction, Rigidbody2D.velocity.y);

            }
        }
        else
        {
            Rigidbody2D.velocity = Vector2.zero;
            CurrentState = EEnemyState.Idle;
        }
    }

    protected override void UpdateAttack()
    {
        if (!IsPlayerOnFollowRange)
        {
            CurrentState = EEnemyState.Follow;
        }
        else if (CanAttack)
        {
            AttackCooldownTimer = AttackCooldown;
            Animator.SetTrigger(AttackAnimationTrigger);

            GameObject knife = Instantiate(KnifePrefab, Head.transform.position, Quaternion.identity);
            knife.GetComponent<ThrowableKnife>().Throw(Direction, _enemyHurtbox);
        }

    }

}
