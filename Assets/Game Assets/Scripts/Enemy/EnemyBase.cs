using IronTools.Attributes;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyBase : MonoBehaviour,ILevelInitializable
{
    [Header("Movement")]
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected bool movingRight = true;
    [SerializeField] protected float rayDistance = 1f;
    [Space(10)]
    [Header("Enemy Can Attack?")]
    [SerializeField] private bool canAttack;
    [ShowIf("canAttack")]
    [SerializeField] protected float detectionRange = 5f;
    [ShowIf("canAttack")]
    [SerializeField] protected float attackRange = 1f;
    [ShowIf("canAttack")]
    [SerializeField] protected LayerMask playerLayer;
    [ShowIf("canAttack")]
    [SerializeField] protected string attackAnimName = "Attack";
    [ShowIf("canAttack")]
    [SerializeField] private float attackCooldown = 1.5f;
    [ShowIf("canAttack")]
    [Range(1,100),SerializeField] private int damageAmount = 20;

    private float lastAttackTime;

    protected Transform player;
    protected bool isPlayerDetected;
    protected Animator anim;
    protected bool canMove;

    private IGameStateService _gameStateService;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        canMove = true;
    }
    public void Initialize(IGameStateService gameStateService)
    {
        _gameStateService =  gameStateService;
        _gameStateService.OnStateChanged += HandleStateChanged;

        canMove = (_gameStateService.CurrentState == GameState.Paused);
    }
    private void HandleStateChanged(GameState newState)
    {
        canMove = (newState != GameState.Paused);
    }
    //Only Patrol
    protected virtual void Patrol()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);

        Vector2 position = transform.position;
        Vector2 frontOrigin = position + Vector2.right * (movingRight ? 0.5f : -0.5f);

        RaycastHit2D groundCheck = Physics2D.Raycast(frontOrigin, Vector2.down, rayDistance);

        if (groundCheck.collider == null)
        {
            Flip();
            return;
        }

        RaycastHit2D wallCheck = Physics2D.Raycast(frontOrigin, direction, 0.1f);
        if (wallCheck.collider != null && !wallCheck.collider.CompareTag("Player"))
        {
            Flip();
        }
    }
    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (movingRight ? 1 : -1);
        transform.localScale = scale;
    }
    //Patrol and Attack Player
    protected virtual void SearchForPlayer()
    {
        Transform detectedPlayer = ScanForPlayer();

        if (detectedPlayer != null && IsPlayerInSight(detectedPlayer))
        {
            float distance = Vector2.Distance(transform.position, detectedPlayer.position);
            if (distance <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack(detectedPlayer);
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                Chase(detectedPlayer);
            }
        }
        else
        {
            Patrol();
        }
    }
    private Transform ScanForPlayer()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        if (hitPlayer != null)
        {
            return hitPlayer.transform;
        }
        return null;
    }
    private bool IsPlayerInSight(Transform targetPlayer)
    {
        Vector2 directionToPlayer = targetPlayer.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, detectionRange, playerLayer);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }
    private void Chase(Transform targetPlayer)
    {
        Vector2 dir = (targetPlayer.position - transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime);

        if ((targetPlayer.position.x > transform.position.x && !movingRight) || (targetPlayer.position.x < transform.position.x && movingRight))
        {
            Flip();
        }
    }
    private void Attack(Transform player)
    {
        if (anim != null) anim.SetTrigger(attackAnimName);

        if (player.TryGetComponent<IHealth>(out var health))
        {
            health.TakeDamage(damageAmount);
        }
        Debug.Log("Attacking player!");
    }
    private void OnDrawGizmosSelected()
    {
        if(canAttack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.orange;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
    protected virtual void Update()
    {
        //Update
    }
}