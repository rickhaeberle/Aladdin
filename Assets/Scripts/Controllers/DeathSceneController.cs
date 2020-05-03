using UnityEngine;

public class DeathSceneController : MonoBehaviour {
    public AudioClip BoxingBellSfx;
    public float TimeToReload;

    private Animator _animator;

    private float _timeToReloadTimer;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    void Start() {
        _animator.SetTrigger("Death");

    }

    private void Update() {
        if (_timeToReloadTimer < TimeToReload) {
            _timeToReloadTimer += Time.deltaTime;
        } else {
            AudioSource.PlayClipAtPoint(BoxingBellSfx, transform.position, GameManager.Instance.Volume);
            GameManager.Instance.Restart();

        }
    }
}
