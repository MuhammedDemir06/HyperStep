using UnityEngine;
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Player Animation Settings")]
    [SerializeField] private Animator playerAnim;
    public void MoveAnim(float inputX)
    {
        playerAnim.SetFloat("Move", inputX);
    }
    public void Jump()
    {
        playerAnim.SetTrigger("Jump");
    }
    public void Death()
    {
        playerAnim.SetTrigger("Death");
    }
    public void Grounded(bool value)
    {
        playerAnim.SetBool("IsGrounded", value);
    }
}
