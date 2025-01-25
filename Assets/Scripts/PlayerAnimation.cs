using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

    public Animator _animator => animator;

    public void WalkSequence(bool walk)
    {
        animator.SetBool("isWalking", walk);
    }

    public void Jumping(bool jump)
    {
        animator.SetBool("isJumping", jump);
    }

    public void isFalling(bool falling)
    {
        animator.SetBool("isFalling", falling);
    }
}
