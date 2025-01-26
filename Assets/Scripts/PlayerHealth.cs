using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerHealthPoint;
    [SerializeField] private GameObject healthBar;

    public int _playerHealthPoint => playerHealthPoint;
    // Start is called before the first frame update

    private void Update()
    {
        float remainHealth = (float)playerHealthPoint / 100 ;
        Vector3 scale = new Vector3(remainHealth, healthBar.transform.lossyScale.y, healthBar.transform.lossyScale.y);
        healthBar.transform.localScale = scale;
    }
    public void DecreaseHealth(int damage)
    {
        playerHealthPoint -= damage;

        if(playerHealthPoint < 0)
        {
            playerHealthPoint = 0;
        }
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
