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
  //  public Animator PlayerAnim;

    private IState currentState;
    private Rigidbody2D rb;
    public Rigidbody2D Rb { get { return rb; } }

    private bool canMove;
    public bool CanMove { get { return canMove; } }
    private IInputProvider _inputProvider;
    private IGameStateService _gameStateService;
    private PlayerAnimationController _playerAnimationController;
    public PlayerAnimationController PlayerAnimation {  get { return _playerAnimationController; } }
    public GameState CurrentState => throw new NotImplementedException();

    //==========Cache============
    public IState NewIdleState;
    public IState NewWalkingState;
    public IState NewJumpingState;
    public IState NewDeathState;
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();

        NewIdleState = new IdleState();
        NewWalkingState = new WalkingState();
        NewJumpingState = new JumpingState();
        NewDeathState = new DeathState();

        currentState = NewIdleState;
        currentState.EnterState(this);

        canMove = true;
    }
    public void Construct(IInputProvider provider,IGameStateService gameState,PlayerAnimationController playerAnimationController)
    {
        _gameStateService = gameState;
        _inputProvider = provider;
        _playerAnimationController = playerAnimationController;

        _inputProvider.OnJump += Jump;
        _gameStateService.OnStateChanged += HandleChangeState;
    }
    private void Move()
    {
        var input = _inputProvider.InputX;

        if (!canMove || IsDead)
            return;

        rb.linearVelocity = new Vector2(moveSpeed * input, rb.linearVelocity.y);
        IsWalking = input != 0;

        _playerAnimationController.MoveAnim(input);

        SetDirection(input);
    }
    private void Update()
    {
        Move();

        currentState.UpdateState(this);
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
}