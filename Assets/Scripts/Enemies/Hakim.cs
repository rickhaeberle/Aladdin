using UnityEngine;

public class Hakim : Enemy
{

    private const string IdleAnimationTrigger = "Idle";
    private const string WalkAnimationTrigger = "Walk";
    private const string AttackAnimationTrigger = "Attack";

    protected override bool CanWalk {
        get {

            bool hasGound = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundDetector.transform.position, 0.1f);
            foreach (Collider2D collider in colliders) {

                FireGround fireGround = collider.GetComponent<FireGround>();
                if (fireGround != null)
                    return false;

                if (collider.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                    hasGound = true;
                }
            }

            return hasGound;
        }
    }

    protected override void UpdateIdle()
    {
        if (IsPlayerOnFollowRange && CanWalk)
        {
            CurrentState = EEnemyState.Follow;

        }
        else
        {
            Animator.SetTrigger(IdleAnimationTrigger);

        }
    }

    protected override void UpdateFollow()
    {
        if (IsPlayerOnFollowRange)
        {
            if (!CanWalk)
            {
                Rigidbody2D.velocity = Vector2.zero;
                CurrentState = EEnemyState.Idle;
            }
            else
            {
                Animator.SetTrigger(WalkAnimationTrigger);

                Rigidbody2D.velocity = new Vector2(MoveSpeed * Direction, Rigidbody2D.velocity.y);

                if (IsPlayerOnAttackRange)
                {
                    Rigidbody2D.velocity = Vector2.zero;
                    CurrentState = EEnemyState.Attack;

                }
            }
        }
        else
        {
            TeleportToStartPosition();

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
        else if (CanAttack)
        {
            CurrentState = EEnemyState.Follow;

        }
    }


}
