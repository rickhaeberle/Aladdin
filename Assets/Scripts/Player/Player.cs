using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour {
    private const string HIT_ANIM = "Hit";
    private const string RESPAWN_ANIM = "Respawn";

    private int MaxHealth = 8;
    private int Apples = 10;

    public float WalkSpeed;
    public float ClimbSpeed;

    public float IdleAnimationCooldown;

    public float JumpForce;

    public float InvulnerableCooldown;

    public GameObject ThrowableApplePrefab;

    public Transform Head;
    public Transform Feet;

    public AudioClip LowAttackSfx;
    public AudioClip HighAttackSfx;
    public AudioClip ThrowObjectSfx;
    public AudioClip HurtSfx;
    public AudioClip WowSfx;
    public AudioClip ItemBoughtSfx;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private bool _isRespawning;

    private Vector2 _direction;

    private int _currentHP;

    private EPlayerState _currentState;

    private bool _isJumping;

    public bool IsThrowing;
    public bool IsAttacking;

    private float _idleAnimationTimeCounter;
    private float _invulnerabilityTimeCounter;

    private float _animatorSpeed;
    private float _gravityScale;

    private float _groundDetectionCooldown;

    private GameObject _rope;
    private GameObject _trunk;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animatorSpeed = _animator.speed;
        _gravityScale = _rigidbody2D.gravityScale;

        _direction = Vector2.right;
        _currentHP = MaxHealth;

        _isRespawning = false;

        UpdateHUD();
    }

    private void Update() {
        if (_isRespawning)
            return;

        if (_isJumping) {
            if (_groundDetectionCooldown > 0) {
                _groundDetectionCooldown -= Time.deltaTime;
            } else if (IsGrounded()) {
                _isJumping = false;
                _animator.SetBool("IsJumping", false);
            }
        }

        UpdateDirection();
        UpdateInvulnerableStatus();

        bool executeAction = UpdateAction();
        if (executeAction) {
            _idleAnimationTimeCounter = 0;
            return;
        }

        GameObject element = GetElement();
        if (element != null)
            return;

        switch (_currentState) {
            case EPlayerState.Idle:
                UpdateIdle();
                break;
            case EPlayerState.Running:
                UpdateRunning();
                break;
            case EPlayerState.LookingUp:
                UpdateLookingUp();
                break;
            case EPlayerState.LookingDown:
                UpdateLookingDown();
                break;
            case EPlayerState.Climbing:
                UpdateClimbing();
                break;
            case EPlayerState.Holding:
                UpdateHolding();
                break;
        }

    }


    private void GoToState(EPlayerState newState) {
        _animator.SetBool("IsJumping", false);

        _animator.SetBool("IsIdle", false);
        _animator.SetBool("IsRunning", newState == EPlayerState.Running);
        _animator.SetBool("IsLookingUp", newState == EPlayerState.LookingUp);
        _animator.SetBool("IsLookingDown", newState == EPlayerState.LookingDown);
        _animator.SetBool("IsClimbing", newState == EPlayerState.Climbing);
        _animator.SetBool("IsHolding", newState == EPlayerState.Holding);

        _animator.speed = _animatorSpeed;
        _rigidbody2D.gravityScale = _gravityScale;

        switch (newState) {
            case EPlayerState.Idle:
                _idleAnimationTimeCounter = 0;
                break;
            case EPlayerState.Climbing: {
                    GameObject rope = GetRope();

                    Vector2 newPosition = transform.position;
                    newPosition.x = rope.transform.position.x;
                    transform.position = newPosition;

                    _isJumping = false;

                    _animator.speed = 0;
                    _rigidbody2D.gravityScale = 0;
                    _rigidbody2D.velocity = Vector2.zero;
                    break;
                }

            case EPlayerState.Holding: {
                    Vector2 newPosition = transform.position;
                    newPosition.y = _trunk.transform.position.y - transform.lossyScale.y + 0.25f;
                    transform.position = newPosition;

                    _isJumping = false;

                    _animator.speed = 0;
                    _rigidbody2D.gravityScale = 0;
                    _rigidbody2D.velocity = Vector2.zero;
                    break;
                }
        }

        Debug.Log($"Changing from {_currentState} to {newState}");
        _currentState = newState;
    }

    private void UpdateIdle() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (IsGrounded()) {
            if (Input.GetKey(KeyCode.UpArrow)) {
                bool isBuying = IsBuying();
                if (!isBuying) {
                    GoToState(EPlayerState.LookingUp);
                }
            } else if (Input.GetKey(KeyCode.DownArrow)) {
                GoToState(EPlayerState.LookingDown);
            } else if (horizontalInput != 0) {
                GoToState(EPlayerState.Running);
            } else {
                if (_idleAnimationTimeCounter < IdleAnimationCooldown) {
                    _idleAnimationTimeCounter += Time.deltaTime;
                } else {
                    _animator.SetBool("IsIdle", true);
                }
            }
        } else {
            _rigidbody2D.velocity = new Vector2(horizontalInput * WalkSpeed, _rigidbody2D.velocity.y);

        }
    }

    private void UpdateRunning() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput == 0 && !_isJumping) {
            GoToState(EPlayerState.Idle);
        } else {
            _rigidbody2D.velocity = new Vector2(horizontalInput * WalkSpeed, _rigidbody2D.velocity.y);
        }
    }

    private void UpdateLookingUp() {
        if (!Input.GetKey(KeyCode.UpArrow)) {
            GoToState(EPlayerState.Idle);
        }
    }

    private void UpdateLookingDown() {
        if (!Input.GetKey(KeyCode.DownArrow)) {
            GoToState(EPlayerState.Idle);
        }
    }

    private void UpdateClimbing() {
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (_isJumping) {
            if (_rigidbody2D.velocity.y <= 0) {
                _rigidbody2D.gravityScale = 0;

                _isJumping = false;
                _animator.SetBool("IsJumping", false);

            }
        } else if (IsAttacking || IsThrowing) {
            _animator.speed = _animatorSpeed;
            _rigidbody2D.velocity = Vector2.zero;

        } else {
            GameObject rope = GetRope();
            if (rope == null) {
                GoToState(EPlayerState.Idle);

            } else {
                if (IsOnCeiling() && verticalInput == 1) {
                    verticalInput = 0;
                }

                _animator.speed = _animatorSpeed * Mathf.Abs(verticalInput);
                _rigidbody2D.velocity = Vector2.up * ClimbSpeed * verticalInput;
            }
        }
    }

    private void UpdateHolding() {

        if (IsAttacking || IsThrowing) {
            _animator.speed = _animatorSpeed;
            _rigidbody2D.velocity = Vector2.zero;

        } else {
            bool isOnTrunk = false;

            RaycastHit2D[] raycastHit2DTrunk = Physics2D.RaycastAll(Head.position, Vector2.up, 1f);
            foreach (RaycastHit2D hit in raycastHit2DTrunk) {
                Trunk trunk = hit.collider.GetComponent<Trunk>();
                if (trunk != null) {
                    isOnTrunk = true;
                    break;
                }
            }

            if (!isOnTrunk) {
                OnTrunkExit();
            } else {
                float horizontalInput = Input.GetAxisRaw("Horizontal");

                _animator.speed = _animatorSpeed * Mathf.Abs(horizontalInput);
                _rigidbody2D.velocity = Vector2.right * ClimbSpeed * horizontalInput;
            }
        }
    }

    private void Jump() {
        if (IsAttacking) {
            return;
        }

        if (_isJumping)
            return;

        _animator.speed = _animatorSpeed;
        _rigidbody2D.gravityScale = _gravityScale;

        switch (_currentState) {
            case EPlayerState.Idle:
            case EPlayerState.Running:
                _isJumping = true;
                _groundDetectionCooldown = 0.5f;
                _animator.SetBool("IsJumping", true);
                break;
            case EPlayerState.Climbing:
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                if (horizontalInput != 0) {
                    GoToState(EPlayerState.Running);
                    Jump();
                    return;
                } else {
                    _isJumping = true;
                    _animator.SetBool("IsJumping", true);
                }
                break;
            case EPlayerState.Holding:
                OnTrunkExit();
                return;

            case EPlayerState.LookingUp:
            case EPlayerState.LookingDown:
                GoToState(EPlayerState.Idle);
                Jump();
                return;
        }

        _rigidbody2D.velocity = Vector2.up * JumpForce;
    }

    private void Attack() {
        if (IsAttacking || IsThrowing) {
            return;
        }

        _animator.speed = _animatorSpeed;
        _animator.SetTrigger("Attack");

        if (_currentState == EPlayerState.LookingUp) {
            AudioSource.PlayClipAtPoint(LowAttackSfx, transform.position, GameManager.Instance.Volume);
        } else {
            AudioSource.PlayClipAtPoint(HighAttackSfx, transform.position, GameManager.Instance.Volume);

        }

    }

    private void Throw() {
        if (IsAttacking || IsThrowing) {
            return;
        }

        if (Apples == 0)
            return;

        Apples--;
        UpdateHUD();

        _animator.speed = _animatorSpeed;
        _animator.SetTrigger("Throw");

        GameObject apple = Instantiate(ThrowableApplePrefab, Head.position, Quaternion.identity);
        apple.GetComponent<AppleProjectile>().ThrowApple(_direction);

        AudioSource.PlayClipAtPoint(ThrowObjectSfx, transform.position, GameManager.Instance.Volume);
    }

    private void UpdateHUD() {
        GameManager.Instance.UpdateHUD(_currentHP, Apples);
    }

    private void UpdateInvulnerableStatus() {
        if (_invulnerabilityTimeCounter > 0) {
            _invulnerabilityTimeCounter -= Time.deltaTime;
            _spriteRenderer.enabled = !_spriteRenderer.enabled;

        } else {
            _spriteRenderer.enabled = true;

        }
    }

    private void UpdateDirection() {
        if (IsAttacking || IsThrowing)
            return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput > 0) {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            _direction = Vector2.right;

        } else if (horizontalInput < 0) {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            _direction = Vector2.left;
        }
    }

    private GameObject GetRope() {
        if (_isJumping && _rigidbody2D.velocity.y > 0)
            return null;

        if (_currentState == EPlayerState.Holding)
            return null;

        return _rope;
    }

    private bool UpdateAction() {
        if (Input.GetKeyDown(KeyCode.C)) {
            _animator.SetBool("IsIdle", false);
            Jump();
            return true;
        } else if (Input.GetKeyDown(KeyCode.X)) {
            _animator.SetBool("IsIdle", false);
            Attack();
            return true;
        } else if (Input.GetKeyDown(KeyCode.Z)) {
            _animator.SetBool("IsIdle", false);
            Throw();
            return true;
        }

        return false;
    }

    private GameObject GetElement() {

        GameObject rope = GetRope();
        if (rope != null) {
            if (EPlayerState.Climbing != _currentState) {
                GoToState(EPlayerState.Climbing);
                return rope;

            }
        }

        return null;
    }

    private bool IsBuying() {
        PeddlerItem peddlerItem = null;

        RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, 0.1f, Vector2.zero);
        foreach (RaycastHit2D raycastHit in raycastHit2Ds) {
            PeddlerItem item = raycastHit.collider.GetComponent<PeddlerItem>();
            if (item != null) {
                peddlerItem = item;
                break;
            }
        }

        if (peddlerItem != null) {
            bool bought = GameManager.Instance.Buy(peddlerItem);
            if (bought) {
                AudioSource.PlayClipAtPoint(ItemBoughtSfx, transform.position, GameManager.Instance.Volume);
                UpdateHUD();

            }

            return true;
        }

        return false;
    }

    public bool IsGrounded() {
        RaycastHit2D raycastHit2DGround = Physics2D.Raycast(Feet.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return raycastHit2DGround.collider != null;
    }

    public bool IsOnCeiling() {
        RaycastHit2D raycastHit2DGround = Physics2D.Raycast(Head.position, Vector2.up, 0.1f, LayerMask.GetMask("Ground"));
        return raycastHit2DGround.collider != null;
    }

    public bool IsLookingUp() {
        return _currentState == EPlayerState.LookingUp;
    }

    public bool IsLookingDown() {
        return _currentState == EPlayerState.LookingDown;
    }

    public void OnCamelJump() {
        _rigidbody2D.velocity = Vector2.up * JumpForce;
    }

    public void OnFlexibleBatonJump() {
        GoToState(EPlayerState.Idle);
        _animator.SetTrigger("Impulse");
        _rigidbody2D.velocity = Vector2.up * JumpForce * 1.25f;
    }

    public bool IsFalling() {
        return _rigidbody2D.velocity.y <= 0;
    }

    public void SetRope(GameObject rope) {
        _rope = rope;
    }

    public void OnTrunkEnter(GameObject trunk) {
        _trunk = trunk;
        GoToState(EPlayerState.Holding);
    }

    public void OnTrunkExit() {
        _trunk = null;
        _isJumping = false;
        GoToState(EPlayerState.Running);
    }

    public void TakeDamage() {
        if (_currentHP < 0 || _invulnerabilityTimeCounter > 0)
            return;

        _idleAnimationTimeCounter = 0;

        _currentHP--;
        UpdateHUD();

        if (_currentHP <= 0) {
            GameManager.Instance.GoToDeathScene();

        } else {
            _invulnerabilityTimeCounter = InvulnerableCooldown;
            _animator.SetTrigger(HIT_ANIM);
            AudioSource.PlayClipAtPoint(HurtSfx, transform.position, GameManager.Instance.Volume);

        }
    }

    public void Stolen() {
        Apples -= 2;
        UpdateHUD();
    }

    public void TakeApple() {
        Apples++;
        UpdateHUD();
    }

    public void TakeJewel() {
        GameManager.Instance.ManageJewels(1);
        UpdateHUD();
    }

    public void TakeGenie() {
        AudioSource.PlayClipAtPoint(WowSfx, transform.position, GameManager.Instance.Volume);

    }

    public void TakeAbu() {
        AudioSource.PlayClipAtPoint(WowSfx, transform.position, GameManager.Instance.Volume);

    }

    public void RecoverHealth() {
        _currentHP = Mathf.Min(MaxHealth, _currentHP + 2);
        UpdateHUD();
    }

    public void RespawnAtCheckpoint(Vector3 checkpoint) {
        _isRespawning = true;

        transform.position = checkpoint;
        _animator.SetTrigger(RESPAWN_ANIM);
    }

    public void OnRespawnEnd() {
        _isRespawning = false;
    }
}
