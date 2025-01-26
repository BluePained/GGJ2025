using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            health.DecreaseHealth(200);
        }
    }
}
