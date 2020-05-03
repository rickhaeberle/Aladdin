using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private const string ACTIVATE_ANIM = "Activate";

    public AudioClip ActivatedSfx;

    private Animator _animator;

    private bool _isActivated;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _isActivated = false;
    }

    private void Start()
    {
        if (GameManager.Instance.IsCheckpointActive(transform.position))
        {
            ActivateCheckpoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActivated)
            return;

        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        ActivateCheckpoint();

        AudioSource.PlayClipAtPoint(ActivatedSfx, transform.position, GameManager.Instance.Volume);

        GameManager.Instance.SetCurrentCheckpoint(transform.position);

    }

    private void ActivateCheckpoint()
    {
        _isActivated = true;
        _animator.SetTrigger(ACTIVATE_ANIM);
    }

}
