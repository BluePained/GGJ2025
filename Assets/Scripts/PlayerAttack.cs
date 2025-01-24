using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

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
