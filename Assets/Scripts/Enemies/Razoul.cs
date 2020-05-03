using UnityEngine;

public class Razoul : Enemy {

    private const string IdleAnimationTrigger = "Idle";
    private const string ProvokeAnimationTrigger = "Provoke";
    private const string WalkAnimationTrigger = "Walk";

    private const string OnFireAnimationTrigger = "OnFire";
    private const string StabAnimationTrigger = "Stab";
    private const string AttackAnimationTrigger = "Attack";

    public AudioClip ProvokeSfx;

    public AudioClip OnFireGround1Sfx;
    public AudioClip OnFireGround2Sfx;

    private bool _isOnFire;
    private float _originalMoveSpeed;

    private float _nextAudioTime;

    protected override void OnAwake() {
        _isOnFire = false;
        _originalMoveSpeed = MoveSpeed;

        _nextAudioTime = Time.time;
    }

    protected override void UpdateIdle() {
        if (IsPlayerOnSightRange) {
            Animator.SetTrigger(ProvokeAnimationTrigger);

            if (IsPlayerOnFollowRange) {
                CurrentState = EEnemyState.Follow;

            }

        } else {
            Animator.SetTrigger(IdleAnimationTrigger);

        }
    }

    protected override void UpdateFollow() {

        if (IsPlayerOnAttackRange && !_isOnFire) {
            Rigidbody2D.velocity = Vector2.zero;
            CurrentState = EEnemyState.Attack;

        } else if (IsPlayerOnFollowRange) {
            if (CanWalk || _isOnFire) {
                CheckFireGround();

                Animator.SetTrigger(WalkAnimationTrigger);
                Animator.SetBool(OnFireAnimationTrigger, _isOnFire);

                Rigidbody2D.velocity = new Vector2(MoveSpeed * Direction, Rigidbody2D.velocity.y);

                if (_isOnFire) {
                    if (Time.time >= _nextAudioTime) {
                        float audioRandom = Random.Range(0, 1f);
                        if (audioRandom >= 0.5f) {
                            AudioSource.PlayClipAtPoint(OnFireGround1Sfx, transform.position, GameManager.Instance.Volume);
                            _nextAudioTime = Time.time + OnFireGround1Sfx.length;

                        } else {
                            AudioSource.PlayClipAtPoint(OnFireGround2Sfx, transform.position, GameManager.Instance.Volume);
                            _nextAudioTime = Time.time + OnFireGround2Sfx.length;

                        }
                    }
                }

            } else {
                Animator.SetTrigger(ProvokeAnimationTrigger);

                float audioRandom = Random.Range(0, 1f);
                if (audioRandom >= 0.9f) {
                    AudioSource.PlayClipAtPoint(ProvokeSfx, transform.position, GameManager.Instance.Volume);
                }

            }

        } else {

            _isOnFire = false;
            CheckDirection = true;
            MoveSpeed = _originalMoveSpeed;
            Animator.SetBool(OnFireAnimationTrigger, false);

            TeleportToStartPosition();

        }
    }

    protected override void UpdateAttack() {
        if (IsPlayerOnAttackRange) {
            if (CanAttack) {
                AttackCooldownTimer = AttackCooldown;

                float attack = Random.Range(0f, 1f);
                if (attack <= 0.7f) {
                    Animator.SetTrigger(AttackAnimationTrigger);

                } else {
                    Animator.SetTrigger(StabAnimationTrigger);

                }
            }
        } else if (CanAttack) {
            CurrentState = EEnemyState.Follow;

        }
    }

    private void CheckFireGround() {
        bool onFireGround = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundDetector.transform.position, 0.25f);
        foreach (Collider2D collider in colliders) {

            FireGround fireGround = collider.GetComponent<FireGround>();
            if (fireGround != null) {
                onFireGround = true;
                break;
            }
        }


        _isOnFire = onFireGround;
        CheckDirection = onFireGround;
        MoveSpeed = onFireGround ? _originalMoveSpeed / 2 : _originalMoveSpeed;

    }
}
