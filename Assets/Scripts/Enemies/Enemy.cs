using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour {

    public float HP;
    public float MoveSpeed;
    public float AttackCooldown;

    public float SightRange;
    public float FollowRange;
    public float AttackRange;

    public GameObject Head;
    public GameObject GroundDetector;

    public GameObject EnemyExplosionEffectPrefab;

    public AudioClip HurtSfx;
    public AudioClip DeadSfx;

    private float _currentHp;
    private Vector3 _startPos;

    protected bool CheckDirection = true;
    protected Vector2 DirectionDefault = Vector2.left;

    protected bool IsTakingDamage { get; private set; }

    protected int Direction;
    protected float AttackCooldownTimer;

    protected Player Player { get; private set; }
    protected Animator Animator { get; private set; }
    protected Rigidbody2D Rigidbody2D { get; private set; }

    protected EEnemyState CurrentState { get; set; }

    protected bool IsPlayerOnSightRange {
        get {
            if (Player == null)
                return false;

            return Vector3.Distance(transform.position, Player.transform.position) <= SightRange;
        }
    }

    protected bool IsPlayerOnFollowRange {
        get {
            if (Player == null)
                return false;

            float myY = transform.position.y;
            float playerY = Player.transform.position.y;

            float yDiff = Mathf.Abs(myY - playerY);
            if (yDiff > 0.35f) {
                return false;
            }

            return Vector3.Distance(transform.position, Player.transform.position) <= FollowRange;
        }
    }

    protected bool IsPlayerOnAttackRange {
        get {
            if (Player == null)
                return false;

            return Vector3.Distance(transform.position, Player.transform.position) <= AttackRange;
        }
    }

    protected bool CanAttack {
        get {
            return AttackCooldownTimer <= 0;
        }
    }

    protected virtual bool CanWalk {
        get {

            bool hasGound = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundDetector.transform.position, 0.1f);
            foreach (Collider2D collider in colliders) {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                    hasGound = true;
                    break;
                }
            }

            return hasGound;
        }
    }

    private void Awake() {
        _currentHp = HP;
        _startPos = transform.position;

        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();

        CurrentState = EEnemyState.Idle;

        Vector3 sightRange = new Vector3(SightRange, 1, 1);
        Head.GetComponent<EnemySight>().transform.localScale = sightRange;

        OnAwake();
    }

    protected virtual void OnAwake() { }

    private void Update() {
        if (CheckDirection)
            UpdateDirection();

        if (AttackCooldownTimer > 0)
            AttackCooldownTimer -= Time.deltaTime;

        switch (CurrentState) {
            case EEnemyState.Idle:
                UpdateIdle();
                break;
            case EEnemyState.Follow:
                UpdateFollow();
                break;
            case EEnemyState.Attack:
                UpdateAttack();
                break;
        }

    }

    protected virtual void UpdateIdle() { }

    protected virtual void UpdateFollow() { }

    protected virtual void UpdateAttack() { }

    private void UpdateDirection() {
        if (Player == null)
            return;

        float playerX = Player.transform.position.x;
        float enemyX = transform.position.x;

        if (playerX <= enemyX) {
            Direction = -1;
        } else {
            Direction = 1;

        }

        if (DirectionDefault == Vector2.left) {
            if (playerX <= enemyX) {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            } else {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        } else {
            if (playerX >= enemyX) {
                transform.localRotation = Quaternion.Euler(0, 0, 0);

            } else {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    protected void TeleportToStartPosition() {
        CurrentState = EEnemyState.Idle;
        Rigidbody2D.velocity = Vector2.zero;
        transform.position = _startPos;
    }

    public void TakeDamage(float damage) {
        _currentHp -= damage;
        if (_currentHp > 0) {
            Animator.SetTrigger("Hit");

            if (HurtSfx != null) {
                AudioSource.PlayClipAtPoint(HurtSfx, transform.position, GameManager.Instance.Volume);
            }

        } else {

            AudioSource.PlayClipAtPoint(DeadSfx, transform.position, GameManager.Instance.Volume);
            Instantiate(EnemyExplosionEffectPrefab, Head.transform.position, Quaternion.identity);

            Destroy(gameObject);

        }
    }

    public void SetPlayer(Player player) {
        Player = player;
    }

    public void OnTakeDamageAnimationStart() {
        IsTakingDamage = true;
    }

    public void OnTakeDamageAnimationEnd() {
        IsTakingDamage = false;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, SightRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, FollowRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
