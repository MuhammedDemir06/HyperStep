using UnityEngine;
using System.Collections;

public class DashState : IState
{
    private readonly float _dashDistance = 4f;   // Kendi ayarladığın değerleri buraya yazabilirsin
    private readonly float _dashDuration = 0.15f;
    private LayerMask _wallLayer;
    public void EnterState(PlayerController player)
    {
        player.PlayerAnimation.DashAnim();

        _wallLayer = player.GroundLayer;

        Vector2 direction = player.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        player.StartCoroutine(DashCoroutine(player, direction));
    }
    private IEnumerator DashCoroutine(PlayerController player, Vector2 direction)
    {
        Vector2 startPos = player.Rb.position;

        Vector2 colliderSize = player.PlayerCollider.size;
        Vector2 colliderCenter = (Vector2)player.transform.position + player.PlayerCollider.offset;

        RaycastHit2D hit = Physics2D.BoxCast(
            colliderCenter,
            colliderSize,
            0f,
            direction,
            _dashDistance,
            _wallLayer);

        Vector2 targetPos;

        if (hit.collider != null)
        {
            float safeDistance = Mathf.Max(hit.distance - 0.05f, 0f);
            targetPos = startPos + direction * safeDistance;
        }
        else
        {
            targetPos = startPos + direction * _dashDistance;
        }

        float timer = 0f;

        player.Rb.linearVelocity = Vector2.zero;

        while (timer < _dashDuration)
        {
            timer += Time.deltaTime;

            player.Rb.MovePosition(Vector2.Lerp(startPos, targetPos, timer / _dashDuration));
            yield return null;
        }

        player.Rb.MovePosition(targetPos);

        if (player.IsGrounded())
        {
            player.ChangePlayerState(player.NewIdleState);
        }
        else
        {
            player.ChangePlayerState(player.NewJumpingState);
        }
    }
    public void UpdateState(PlayerController player)
    {
        //
    }
    public void ExitState(PlayerController player)
    {
       // Debug.Log("Exited Dash State");
    }
}