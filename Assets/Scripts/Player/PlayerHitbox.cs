using UnityEngine;

public class PlayerHitbox : MonoBehaviour {

    public AudioClip ChingSfx;

    private void OnTriggerEnter2D(Collider2D other) {

        EnemyHitbox enemyHitbox = other.GetComponent<EnemyHitbox>();
        if (enemyHitbox != null) {
            AudioSource.PlayClipAtPoint(ChingSfx, transform.position, GameManager.Instance.Volume);

        }

        EnemyHurtbox enemyHurtbox = other.GetComponent<EnemyHurtbox>();
        if (enemyHurtbox != null) {
            enemyHurtbox.TakeDamage(2f);
        }
    }
}
