using UnityEngine;

public class ItemGenie : MonoBehaviour
{
    private const string BLINK_ANIM = "Blink";

    public float BlinkCooldown;
    public float BounceSpeed = 0.2f;

    public GameObject ItemTakenEffectPrefab;
    public AudioClip ItemTakenSfx;

    private Animator _animator;

    private Vector3 _initialPosition;
    private float _blinkCooldownTimer;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _initialPosition = transform.position;

        _blinkCooldownTimer = 0;
    }

    void Update()
    {
        float y = _initialPosition.y + Mathf.PingPong(Time.time * BounceSpeed, 0.1f);
        transform.position = new Vector3(_initialPosition.x, y, _initialPosition.z);

        if (_blinkCooldownTimer < BlinkCooldown)
        {
            _blinkCooldownTimer += Time.deltaTime;
        }
        else
        {
            _blinkCooldownTimer = 0;
            _animator.SetTrigger(BLINK_ANIM);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.TakeGenie();

        AudioSource.PlayClipAtPoint(ItemTakenSfx, transform.position, GameManager.Instance.Volume);
        Instantiate(ItemTakenEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
