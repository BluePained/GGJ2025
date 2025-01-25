using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D rb;

    [Header("Player Animation Data")]
    [SerializeField] private float airAttackAdditional;
    [SerializeField] private float fallAttackAdditional;
    public float _airAttackAdditional => airAttackAdditional;
    public float _fallAttackAdditional => fallAttackAdditional;

    private bool isAttacking;
    public bool _isAttacking => isAttacking;
    public void SimpleAttack()
    {
        if(Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            _animator.SetBool("isAttack", true);
            StartCoroutine(ReverseStateAnimation("isAttack", false));
        }
    }

    public void SimpleAirAttack()
    {
        isAttacking = true;
        _animator.SetBool("isAttack", true);
        StartCoroutine(ReverseStateAnimation("isAttack", false));
    }

    public void FallingAirAttack()
    {
        isAttacking = true;
        _animator.SetBool("isAttack", true);
        StartCoroutine(ReverseStateAnimation("isAttack", false));
    }

    private IEnumerator ReverseStateAnimation(string animation, bool state)
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 0.2f);
        _animator.SetBool(animation, state);
        isAttacking = false;
    }

}
