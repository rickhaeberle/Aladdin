using UnityEngine;

public class Juggler : Enemy
{
    public float LowAttackRange;
    public GameObject KnifePrefab;

    public AudioClip ThrowKnifeSfx;

    private EnemyHurtbox _enemyHurtbox;

    protected override void OnAwake()
    {
        DirectionDefault = Vector2.right;

        _enemyHurtbox = GetComponentInChildren<EnemyHurtbox>();

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

                float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

                GameObject knife = Instantiate(KnifePrefab, Head.transform.position, Quaternion.identity);

                if (distanceToPlayer < LowAttackRange)
                {
                    knife.GetComponent<ThrowableKnife>().Throw(Direction, 3f, 1f, _enemyHurtbox);

                }
                else
                {
                    knife.GetComponent<ThrowableKnife>().Throw(Direction, 1.75f, 5f, _enemyHurtbox);

                }

                AudioSource.PlayClipAtPoint(ThrowKnifeSfx, transform.position, GameManager.Instance.Volume);

            }
        }
        else
        {
            CurrentState = EEnemyState.Idle;

        }
    }
}
