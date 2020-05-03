using UnityEngine;

public class ItemAbu : MonoBehaviour {

    private const string SHINE_ANIM = "Shine";

    public float ShineInterval = 2f;

    public GameObject ItemTakenEffectPrefab;
    public AudioClip ItemTakenSfx;

    private float _shineIntervalTimer;

    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();

        _shineIntervalTimer = 0;
    }

    private void Update() {
        if (_shineIntervalTimer < ShineInterval) {
            _shineIntervalTimer += Time.deltaTime;
        } else {
            _shineIntervalTimer = 0;
            _animator.SetTrigger(SHINE_ANIM);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.TakeAbu();

        AudioSource.PlayClipAtPoint(ItemTakenSfx, transform.position, GameManager.Instance.Volume);
        Instantiate(ItemTakenEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

}
