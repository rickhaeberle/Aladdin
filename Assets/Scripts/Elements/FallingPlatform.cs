using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingPlatform : MonoBehaviour {

    private const string TRIGGER_ANIM = "Trigger";

    public float RespawnRange;

    public float FallSpeed;
    public float FallCooldown;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private PlatformState _platformState;
    private Vector3 _initialPosition;

    private Player _player;

    private float _fallTimer;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        _platformState = PlatformState.Idle;
        _initialPosition = transform.position;

        _fallTimer = 0;
    }

    private void Update() {
        switch (_platformState) {
            case PlatformState.Triggered:
                _fallTimer += Time.deltaTime;
                if (_fallTimer >= FallCooldown) {
                    Fall();

                }

                break;
            case PlatformState.Fallen:
                Respawn();
                break;
        }
    }

    private void Respawn() {
        if (Vector3.Distance(_initialPosition, _player.transform.position) >= RespawnRange) {
            _rigidbody2D.velocity = Vector2.zero;
            _platformState = PlatformState.Idle;
            _fallTimer = 0;

            transform.position = _initialPosition;
            _spriteRenderer.enabled = true;

            _player = null;
        } else if (transform.position.y < -2) {
            _rigidbody2D.velocity = Vector2.zero;

        }
    }

    private void Fall() {
        _animator.SetTrigger(TRIGGER_ANIM);

        _platformState = PlatformState.Fallen;
        _rigidbody2D.velocity = new Vector2(0, -FallSpeed);

    }

    private void OnCollisionEnter2D(Collision2D other) {

        Player player = other.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        if (_platformState == PlatformState.Idle) {
            _player = player;
            _platformState = PlatformState.Triggered;

        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_initialPosition, RespawnRange);
    }

    public void OnPlatformDestroyed() {
        if (_platformState != PlatformState.Idle) {
            _spriteRenderer.enabled = false;

        }
    }
}
