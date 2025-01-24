using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float bubblePopTime;
    [SerializeField] private float bubbleDelay;
    [SerializeField] private Collider2D bubbleCollider;
    [SerializeField] private SpriteRenderer bubbleSprite;     

    private bool isPopping;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPopping)
        {
            StartCoroutine(BubblePop());
        }
    }

    private IEnumerator BubblePop()
    {
        isPopping = true;
        yield return new WaitForSeconds(bubblePopTime);
        bubbleCollider.enabled = false;
        bubbleSprite.enabled = false;

        StartCoroutine(BubbleBack());
    }

    private IEnumerator BubbleBack()
    {
        yield return new WaitForSeconds(bubbleDelay);
        bubbleSprite.enabled = true;
        bubbleCollider.enabled = true;
        isPopping=false;
    }
}
