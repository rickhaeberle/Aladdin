using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FireGround : MonoBehaviour {
    private const string LIGHT_UP_ANIM = "LightUp";

    public float ActivationCooldown;
    public AudioClip FireSfx;

    private float _activationCooldownCounter;

    private FireGroundState _fireGroundState;

    private Player _player;
    private Animator _animator;

    private void Awake() {
        _activationCooldownCounter = 0;
        _fireGroundState = FireGroundState.Idle;

        _animator = GetComponent<Animator>();
    }

    private void Update() {
        switch (_fireGroundState) {
            case FireGroundState.Idle:
                UpdateIdle();
                break;
            case FireGroundState.Triggered:
                UpdateTriggered();
                break;
            case FireGroundState.LightUp:
                UpdateLightUp();
                break;
        }
    }

    private bool IsPlayerOnRange() {
        if (_player == null)
            return false;

        return Vector3.Distance(transform.position, _player.transform.position) <= 0.1f;
    }

    private void UpdateIdle() {
        if (_player != null) {
            _fireGroundState = FireGroundState.Triggered;
        }
    }

    private void UpdateTriggered() {
        if (_activationCooldownCounter < ActivationCooldown) {
            _activationCooldownCounter += Time.deltaTime;

        } else {
            _activationCooldownCounter = 0;
            _fireGroundState = FireGroundState.LightUp;

        }
    }

    private void UpdateLightUp() {
        _animator.SetTrigger(LIGHT_UP_ANIM);
        _fireGroundState = FireGroundState.Idle;

        AudioSource.PlayClipAtPoint(FireSfx, transform.position, GameManager.Instance.Volume);

        if (_player != null) {
            _player.TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Player player = collider.gameObject.GetComponent<Player>();
        if (player != null) {
            _player = player;

        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        Player player = collider.gameObject.GetComponent<Player>();
        if (player != null) {
            _player = null;
        }

    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
