using UnityEngine;

public class ItemJewel : MonoBehaviour {

    private const string SHINE_ANIM = "Shine";

    public AudioClip JewelCollectedSfx;
    public GameObject ItemTakenEffectPrefab;

    public float ShineInterval = 2f;

    private Animator _animator;
    private float _shineIntervalTimer = 0;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        if (_shineIntervalTimer > ShineInterval) {
            _shineIntervalTimer = 0;
            _animator.SetTrigger(SHINE_ANIM);

        } else {
            _shineIntervalTimer += Time.deltaTime;

        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.TakeJewel();

        AudioSource.PlayClipAtPoint(JewelCollectedSfx, transform.position, GameManager.Instance.Volume);
        Instantiate(ItemTakenEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

    }
}

