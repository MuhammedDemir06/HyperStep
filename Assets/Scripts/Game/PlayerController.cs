using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour,IPausable
{
    [Header("Player Controller")]
    [Range(1, 15)]
    [SerializeField] private float moveSpeed = 5f;
    [Range(.1f,3f)]
    [SerializeField] private float groundDistance = 0.4f;
    [Range(1f, 5f)]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float playerScaleX = 1;
    public bool IsWalking;
    [Header("Referances")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public Animator PlayerAnim;

    private IState currentState;
    private Rigidbody2D rb;

    private bool canMove;
    private void OnEnable()
    {
        InputManager.PlayerMove += Move;
        InputManager.PlayerJump += Jump;
    }
    private void OnDisable()
    {
        InputManager.PlayerMove -= Move;
        InputManager.PlayerJump -= Jump;
    }
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();

        currentState = new IdleState();
        currentState.EnterState(this);

        canMove = true;
    }
    private void Move(float input)
    {
        if (!canMove)
            return;

        rb.linearVelocity = new Vector2(moveSpeed * input, rb.linearVelocity.y);
        IsWalking = input != 0;

        PlayerAnim.SetFloat("Move", input);

        SetDirection(input);

       // PlayerAnim.SetBool("IsGrounded",IsGrounded());
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
    private void Jump()
    {
        if (IsGrounded() && canMove)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 10);
        }
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundLayer);
    }
    public void ChangeState(IState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    private void Update()
    {
        currentState.UpdateState(this);
    }
    public void OnPause()
    {
        canMove = false;
        rb.simulated = false;
    }
    public void OnResume()
    {
        canMove = true;
        rb.simulated = true;
    }
}