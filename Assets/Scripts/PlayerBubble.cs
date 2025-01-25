using UnityEngine;

public class PlayerBubble : MonoBehaviour
{
    [SerializeField] private GameObject playerBubble;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerBubbleTransform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float bubblingTime;
    [SerializeField] private float bubblingCounter;

    [SerializeField] private PlayerAnimation _playerAnimation;

    private bool isBubble;
    public bool _isBubble => isBubble;
    // Start is called before the first frame update
    void Start()
    {
        if(_playerAnimation == null)
        {
            _playerAnimation = GetComponent<PlayerAnimation>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2") && !isBubble)
        {
            if(isWall())
            {
                return;
            }

            bubblingCounter += Time.deltaTime;
            _playerAnimation.BubbleBlow();
            if(bubblingCounter >= bubblingTime)
            {
                isBubble = true;
            }
        }
        else
        {
            bubblingCounter = 0;
        }

        if(Input.GetButtonUp("Fire2"))
        {
            isBubble = false;
        }
    }

    private bool isWall()
    {
        bool wall = Physics2D.OverlapCircle(playerBubbleTransform.position, 1.2f, layerMask);
        return wall;
    }
}
