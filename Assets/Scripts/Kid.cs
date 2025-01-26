using UnityEngine;

public class Kid : MonoBehaviour
{
    [SerializeField] private Timer Timer;
    private bool isWin;

    public bool _isWin => isWin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isWin = true;
        }
    }
}
