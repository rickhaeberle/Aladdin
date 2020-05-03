using UnityEngine;

public class Thief : Enemy {

    private const string IDLE_ANIM = "Idle";
    private const string PROVOKE_ANIM = "Provoke";
    private const string WALK_ANIM = "Walk";
    private const string ATTACK_ANIM = "Attack";

    protected override void OnAwake() {
        DirectionDefault = Vector2.right;

    }

    protected override void UpdateIdle() {
        if (IsPlayerOnSightRange) {
            Animator.SetTrigger(PROVOKE_ANIM);

            if (IsPlayerOnFollowRange) {
                CurrentState = EEnemyState.Follow;

            }

        } else {
            Animator.SetTrigger(IDLE_ANIM);

        }
    }

    protected override void UpdateFollow() {

        if (IsPlayerOnAttackRange) {
            Rigidbody2D.velocity = Vector2.zero;
            CurrentState = EEnemyState.Attack;

        } else if (IsPlayerOnFollowRange) {
            if (CanWalk) {

                Animator.SetTrigger(WALK_ANIM);
                Rigidbody2D.velocity = new Vector2(MoveSpeed * Direction, Rigidbody2D.velocity.y);

            } else {
                Animator.SetTrigger(PROVOKE_ANIM);

            }

        } else {
            TeleportToStartPosition();

        }
    }

    protected override void UpdateAttack() {
        if (IsPlayerOnAttackRange) {
            if (CanAttack) {
                AttackCooldownTimer = AttackCooldown;
                Animator.SetTrigger(ATTACK_ANIM);
            }

        } else if (CanAttack) {
            CurrentState = EEnemyState.Follow;

        }
    }

}
