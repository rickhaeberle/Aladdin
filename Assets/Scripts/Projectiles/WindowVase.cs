using UnityEngine;

public class WindowVase : MonoBehaviour {
    private const string SplashAnimaton = "Splash";

    public GameObject ExplosionPrefab;

    public AudioClip SplashSfx;
    public AudioClip ExplosionSfx;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;

    private void Awake() {
        _animator = GetComponent<Animator>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        _rigidbody.velocity = new Vector2(0.2f, _rigidbody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        PlayerHitbox playerHitbox = other.GetComponent<PlayerHitbox>();
        if (playerHitbox != null) {
            AudioSource.PlayClipAtPoint(ExplosionSfx, transform.position, GameManager.Instance.Volume);
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        PlayerHurtbox playerHurtbox = other.GetComponent<PlayerHurtbox>();
        if (playerHurtbox != null) {
            playerHurtbox.TakeDamage();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            AudioSource.PlayClipAtPoint(SplashSfx, transform.position, GameManager.Instance.Volume);

            _rigidbody.velocity = Vector2.zero;
            _rigidbody.gravityScale = 0.0f;
            _collider.enabled = false;

            _animator.SetTrigger(SplashAnimaton);
        }
    }

    public void CleanUp() {
        Destroy(gameObject);

    }

}
