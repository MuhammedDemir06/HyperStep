using UnityEngine;
using IronTools.Attributes;
public class PlayerController : MonoBehaviour,IPlayer
{
    [ShowDivider(EditorColor.Green, "Player Controller")]
    public bool IsDead;
    [Range(1, 15)]
    [SerializeField] private float moveSpeed = 4f;
    [Range(.1f,3f)]
    [SerializeField] private float groundDistance = 0.4f;
    [Range(1f, 5f)]
    [SerializeField] private float jumpForce = 1.6f;
    public float JumpForce { get { return jumpForce; }}

    [SerializeField] private float playerScaleX = 1;

    [HideInInspector] public bool IsWalking;

    [ShowDivider(EditorColor.Green, "Referances")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [ShowDivider(EditorColor.Green, "Dash Setting")]
    [SerializeField] private float dashCooldown = 10f;

    private float currentDashCooldown;
    public bool CanDash => currentDashCooldown >= dashCooldown;

    public LayerMask GroundLayer {  get { return groundLayer; }}

    [Header("Effects")]
    [SerializeField] private TrailRenderer playerDashTrailEffect;

    private CapsuleCollider2D playerCollider;
    public CapsuleCollider2D PlayerCollider { get { return playerCollider; }}

    private IState currentState;
    private Rigidbody2D rb;
    public Rigidbody2D Rb { get { return rb; } }

    private bool canMove;
    private float inputX;
    public bool CanMove { get { return canMove; } }

    private float deathOffset;

    private IInputProvider _inputProvider;
    private IGameStateService _gameStateService;
    private PlayerAnimationController _playerAnimationController;
    private PlayerCameraShake _playerCameraShake;
    public PlayerAnimationController PlayerAnimation {  get { return _playerAnimationController; } }
    public GameState CurrentState => throw new System.NotImplementedException();
    //==========Cache============
    public IState NewIdleState;
    public IState NewWalkingState;
    public IState NewJumpingState;
    public IState NewDeathState;
    public IState NewDashState;
    public IState NewLevelCompleteState;

    public event System.Action<float> OnDashCooldownChanged;

    private void Awake()
    {
        Init();
    }
    private void Update()
    {
        Move();

        currentState.UpdateState(this);
    }
    private void FixedUpdate()
    {
        CheckDeathOffset();
    }
    private void OnDisable()
    {
        if (_gameStateService != null)
        {
            _gameStateService.OnStateChanged -= HandleChangeState;
        }
    }
    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        NewIdleState = new IdleState();
        NewWalkingState = new WalkingState();
        NewJumpingState = new JumpingState();
        NewDeathState = new DeathState();
        NewDashState = new DashState();
        NewLevelCompleteState = new LevelCompleteState();

        currentState = NewIdleState;
        currentState.EnterState(this);

        canMove = true;

        currentDashCooldown = dashCooldown;
    }
    public void Construct(IInputProvider provider,IGameStateService gameState,PlayerAnimationController playerAnimationController,PlayerCameraShake playerCameraShake)
    {
        _gameStateService = gameState;
        _inputProvider = provider;
        _playerAnimationController = playerAnimationController;
        _playerCameraShake = playerCameraShake;

        _inputProvider.OnJump += Jump;
        _gameStateService.OnStateChanged += HandleChangeState;
        _inputProvider.OnDash += Dash;

        //
        OnDashCooldownChanged?.Invoke(dashCooldown);
    }
    public void SetPlayerStartPos(Vector3 pos)
    {
        transform.position = pos;

        playerDashTrailEffect.emitting = false;
    }
    public void SetPlayerMoveState()
    {
        rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
        canMove = false;
    }
    public void SetDeathOffset(float value)
    {
        deathOffset = value;
    }
    private void CheckDeathOffset()
    {
        if (transform.position.y < deathOffset)
        {
            _gameStateService.ChangeState(GameState.Death);
        }
    }
    private void Move()
    {
        inputX = _inputProvider.InputX;

        _playerAnimationController.MoveAnim(inputX);

        IsWalking = inputX != 0;

        if (!canMove || IsDead)
            return;

        rb.linearVelocity = new Vector2(moveSpeed * inputX, rb.linearVelocity.y);

        SetDirection(inputX);
    }
    private void SetDirection(float input)
    {
        var newScale = transform.localScale;

        if (input > 0)
            newScale.x = playerScaleX;
        else if (input < 0)
            newScale.x = -playerScaleX;

        transform.localScale = newScale;
    }
    public void Jump()
    {
        if (!canMove || IsDead) return;

        ChangePlayerState(NewJumpingState);
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundLayer);
    }
    public void ChangePlayerState(IState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    public void HandleChangeState(GameState newState)
    {
        canMove = (newState != GameState.Paused);
        rb.simulated = canMove;

        if (newState == GameState.Death)
            ChangePlayerState(NewDeathState);

        if (newState == GameState.Finished)
            ChangePlayerState(NewLevelCompleteState);
    }
    public void Dash()
    {
        if (!canMove || IsDead || inputX == 0 || !CanDash)
            return;

        _playerCameraShake.TriggerShake(CameraShakeType.Medium);
        ChangePlayerState(NewDashState);

        StartCoroutine(DashCooldownRoutine());
    }
    private System.Collections.IEnumerator DashCooldownRoutine()
    {
        playerDashTrailEffect.emitting = true;

        currentDashCooldown = 0;

        while (dashCooldown > currentDashCooldown)
        {
            currentDashCooldown += Time.deltaTime;

            OnDashCooldownChanged?.Invoke(currentDashCooldown / dashCooldown);

            yield return null;
        }

        currentDashCooldown = dashCooldown;

        OnDashCooldownChanged?.Invoke(dashCooldown);

        playerDashTrailEffect.emitting = false;
    }
}