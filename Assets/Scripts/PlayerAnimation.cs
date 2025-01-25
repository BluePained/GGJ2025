using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void SimpleAttack()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            _animator.SetBool("isAttack", true);
            StartCoroutine(ReverseStateAnimation("isAttack", false));
        }
    }

    private IEnumerator ReverseStateAnimation(string animation, bool state)
    {
        print(_animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 0.2f);
        _animator.SetBool(animation, state);
    }
}
