using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    [Range(1, 15)]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform groundCheck;
    [Range(.1f,3f)]
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [Range(1f, 5f)]
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody2D rb;

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
        rb = GetComponent<Rigidbody2D>();
    }
    private void Move(float input)
    {
        rb.linearVelocity = new Vector2(moveSpeed * input, rb.linearVelocity.y);
    }
    private void Jump()
    {
        if(IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 10);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundLayer);
    }
}