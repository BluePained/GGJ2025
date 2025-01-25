using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerHealthPoint;

    public int _playerHealthPoint => playerHealthPoint;
    // Start is called before the first frame update
    public void DecreaseHealth(int damage)
    {
        playerHealthPoint -= damage;
    }

    public void IncreaseHealth(int heal)
    {
        playerHealthPoint += heal;
    }

    public int GetHealth()
    {
        return playerHealthPoint;
    }
    
}
