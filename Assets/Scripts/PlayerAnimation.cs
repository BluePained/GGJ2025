using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D rb;

    [Header("Player Animation Data")]
    [SerializeField] private float airAttackAdditional;
    [SerializeField] private float fallAttackAdditional;

    public Animator _animatorController => _animator;
    public float _airAttackAdditional => airAttackAdditional;
    public float _fallAttackAdditional => fallAttackAdditional;

    private bool isAttacking;
    public bool _isAttacking => isAttacking;

    private bool isBubbling;
    public bool _isBubbling => isBubbling;

    public void BubbleBlow()
    {
        isBubbling = true;
        _animator.SetBool("isBubbleBlowing", true);
        StartCoroutine(ReverseStateAnimation("isBubbleBlowing", false));
    }

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
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        _animator.SetBool(animation, state);
        isAttacking = false;
        isBubbling = false;
    }

}
