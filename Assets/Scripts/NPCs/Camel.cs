using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(PlatformEffector2D))]
public class Camel : MonoBehaviour
{
    private const string ACTIVE_ANIM = "Active";

    public GameObject SpitPrefab;
    public GameObject Mouth;

    public AudioClip SpitSfx;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        Vector3 collisionDirection = ( other.gameObject.transform.position - gameObject.transform.position ).normalized;
        if (collisionDirection.y > 0.5f)
        {
            _animator.SetTrigger(ACTIVE_ANIM);

            player.OnCamelJump();
        }
    }

    public void OnSpit()
    {
        GameObject spit = Instantiate(SpitPrefab, Mouth.transform.position, Quaternion.identity);
        spit.GetComponent<Spit>().Throw();

        AudioSource.PlayClipAtPoint(SpitSfx, transform.position, GameManager.Instance.Volume);
    }
}