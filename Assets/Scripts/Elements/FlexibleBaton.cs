using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(PlatformEffector2D))]
public class FlexibleBaton : MonoBehaviour
{
    private const string TriggerAnimation = "Trigger";

    public AudioClip ActivatedSfx;

    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionStay2D(Collision2D  other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        if(player.IsFalling())
        {
            _animator.SetTrigger(TriggerAnimation);

            AudioSource.PlayClipAtPoint(ActivatedSfx, transform.position, GameManager.Instance.Volume);

            player.OnFlexibleBatonJump();
        }
    }

}
