using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AppleProjectile : MonoBehaviour {
    public float XSpeed;
    public float YSpeed;

    public GameObject SplashEffectPrefab;

    public AudioClip SplashSfx;

    private Rigidbody2D _rigidbody2D;

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();

    }

    public void ThrowApple(Vector2 direction) {
        _rigidbody2D.velocity = new Vector2(direction.x * XSpeed, YSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        EnemyHitbox enemyHitbox = other.GetComponent<EnemyHitbox>();
        if (enemyHitbox != null) {
            Destroy(gameObject);
            return;
        }

        EnemyHurtbox enemyHurtbox = other.GetComponent<EnemyHurtbox>();
        if (enemyHurtbox != null) {
            enemyHurtbox.TakeDamage(0.75f);

            Hit();

        } else if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            Hit();

        }
    }

    private void Hit() {
        AudioSource.PlayClipAtPoint(SplashSfx, transform.position, GameManager.Instance.Volume);
        Instantiate(SplashEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
