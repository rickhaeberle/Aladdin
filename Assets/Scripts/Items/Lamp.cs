using UnityEngine;

public class Lamp : MonoBehaviour {
    public float Range;
    public float Damage;

    public GameObject ItemTakenEffectPrefab;
    public AudioClip ItemTakenSfx;

    private void OnTriggerEnter2D(Collider2D other) {

        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, Range, Vector2.zero);
        foreach (RaycastHit2D raycastHit in raycastHit2Ds) {
            EnemyHurtbox enemyHurtbox = raycastHit.collider.GetComponent<EnemyHurtbox>();
            if (enemyHurtbox == null)
                continue;

            enemyHurtbox.TakeDamage(Damage);

        }

        AudioSource.PlayClipAtPoint(ItemTakenSfx, transform.position, GameManager.Instance.Volume);
        Instantiate(ItemTakenEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
