using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyBase : MonoBehaviour,IPausable
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
    private float lastAttackTime;

    protected Transform player;
    protected bool isPlayerDetected;
    protected Animator anim;
    protected bool canMove;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();

        canMove = true;
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

        RaycastHit2D wallCheck = Physics2D.Raycast(frontOrigin, direction, 0.2f);
        if (wallCheck.collider != null)
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
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null && IsPlayerInSight())
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                Chase();
            }
        }
        else
        {
            Patrol();
        }
    }
    private bool IsPlayerInSight()
    {
        Vector2 directionToPlayer = player.position - transform.position;

        if (directionToPlayer.magnitude > detectionRange)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, detectionRange, playerLayer);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }
    private void Chase()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime);

        if ((player.position.x > transform.position.x && !movingRight) || (player.position.x < transform.position.x && movingRight))
        {
            Flip();
        }
    }
    private void Attack()
    {
        anim.SetTrigger(attackAnimName);
        Debug.Log("Attacking player!");
    }
    protected virtual void Update()
    {
        //Update
    }
    public void OnPause()
    {
        canMove = false;
    }
    public void OnResume()
    {
        canMove = true;
    }
}