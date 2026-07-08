using UnityEngine;
using IronTools.Attributes;
using System;
public class PlayerController : MonoBehaviour
{
    [ShowDivider(EditorColor.Green, "Player Controller")]
    public bool IsDead;
    [Range(1, 15)]
    [SerializeField] private float moveSpeed = 5f;
    [Range(.1f,3f)]
    [SerializeField] private float groundDistance = 0.4f;
    [Range(1f, 5f)]
    [SerializeField] private float jumpForce = 5f;
    public float JumpForce { get { return jumpForce; }}

    [SerializeField] private float playerScaleX = 1;

    [HideInInspector] public bool IsWalking;

    [ShowDivider(EditorColor.Green, "Referances")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public LayerMask GroundLayer {  get { return groundLayer; }}

    private CapsuleCollider2D playerCollider;
    public CapsuleCollider2D PlayerCollider { get { return playerCollider; }}

    private IState currentState;
    private Rigidbody2D rb;
    public Rigidbody2D Rb { get { return rb; } }

    private bool canMove;
    private float inputX;
    public bool CanMove { get { return canMove; } }
    private IInputProvider _inputProvider;
    private IGameStateService _gameStateService;
    private PlayerAnimationController _playerAnimationController;
    private PlayerCameraShake _playerCameraShake;
    public PlayerAnimationController PlayerAnimation {  get { return _playerAnimationController; } }
    public GameState CurrentState => throw new NotImplementedException();

    //==========Cache============
    public IState NewIdleState;
    public IState NewWalkingState;
    public IState NewJumpingState;
    public IState NewDeathState;
    public IState NewDashState;
    private void Awake()
    {
        Init();
    }
    private void Update()
    {
        Move();

        currentState.UpdateState(this);
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

        currentState = NewIdleState;
        currentState.EnterState(this);

        canMove = true;
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
    }
    private void Move()
    {
        inputX = _inputProvider.InputX;

        if (!canMove || IsDead)
            return;

        rb.linearVelocity = new Vector2(moveSpeed * inputX, rb.linearVelocity.y);
        IsWalking = inputX != 0;

        _playerAnimationController.MoveAnim(inputX);

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
    }
    public void Dash()
    {
        if (!canMove || IsDead || inputX == 0) return;

        _playerCameraShake.TriggerShake(CameraShakeType.Medium);
        ChangePlayerState(NewDashState);
    }
}