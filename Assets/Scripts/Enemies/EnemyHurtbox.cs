using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    public bool DamagePlayer = true;

    private Enemy _owner;

    private void Awake()
    {
        _owner = GetComponentInParent<Enemy>();
    }

    public void TakeDamage(float damage)
    {
        _owner.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!DamagePlayer)
            return;

        PlayerHurtbox playerHurtbox = other.GetComponent<PlayerHurtbox>();
        if (playerHurtbox != null)
        {
            playerHurtbox.TakeDamage();
        }
    }

}
