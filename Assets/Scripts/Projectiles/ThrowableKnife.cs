using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class ThrowableKnife : MonoBehaviour
{

    public float XSpeed;
    public float YSpeed;

    public float XSpeedReflect = 2f;
    public float YSpeedReflect = 2f;

    public AudioClip ChingSfx;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    private bool _canHurtOwner;

    private int _originalDirection;
    private EnemyHurtbox _owner;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _owner = null;

        _canHurtOwner = false;
    }

    public void Throw(int direction, EnemyHurtbox owner)
    {
        _originalDirection = direction;
        _owner = owner;

        _rigidbody2D.velocity = new Vector2(_originalDirection * XSpeed, YSpeed);
    }

    public void Throw(int direction, float xForce, float yForce, EnemyHurtbox owner)
    {
        _originalDirection = direction;
        _owner = owner;

        _rigidbody2D.velocity = new Vector2(_originalDirection * xForce, yForce);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        PlayerHurtbox playerHurtbox = other.GetComponent<PlayerHurtbox>();
        if (playerHurtbox != null)
        {
            playerHurtbox.TakeDamage();
            Destroy(gameObject);
            return;
        }

        PlayerHitbox playerHitbox = other.GetComponent<PlayerHitbox>();
        if (playerHitbox != null && !_canHurtOwner)
        {
            _canHurtOwner = true;
            _rigidbody2D.velocity = new Vector2(_originalDirection * XSpeedReflect * -1, YSpeedReflect);
            AudioSource.PlayClipAtPoint(ChingSfx, transform.position, GameManager.Instance.Volume);

            return;
        }

        if (_canHurtOwner)
        {
            EnemyHurtbox enemyHurtbox = other.GetComponent<EnemyHurtbox>();
            if (enemyHurtbox != null && enemyHurtbox == _owner)
            {
                enemyHurtbox.TakeDamage(1f);
                Destroy(gameObject);
                return;
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            AudioSource.PlayClipAtPoint(ChingSfx, transform.position, GameManager.Instance.Volume);
            Destroy(gameObject);

        }

    }
}
