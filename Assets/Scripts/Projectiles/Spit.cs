using UnityEngine;

public class Spit : MonoBehaviour
{
    public float Force = 80f;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        EnemyHurtbox enemyHurtbox = other.GetComponent<EnemyHurtbox>();
        if (enemyHurtbox != null)
        {

            enemyHurtbox.TakeDamage(1f);
            Destroy(gameObject);


        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);

        }
    }

    public void Throw()
    {
        _rigidbody.AddForce(Vector2.right * Force);

    }

}
