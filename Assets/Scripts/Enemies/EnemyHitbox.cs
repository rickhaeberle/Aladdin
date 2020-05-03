using UnityEngine;

public class EnemyHitbox : MonoBehaviour {

    public bool IsThief = false;

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHurtbox playerHurtbox = other.GetComponent<PlayerHurtbox>();
        if (playerHurtbox != null) {
            if (IsThief) {
                playerHurtbox.Stolen();

            } else {
                playerHurtbox.TakeDamage();

            }

        }

    }

}
