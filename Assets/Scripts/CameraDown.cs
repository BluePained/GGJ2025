using UnityEngine;

public class CameraDown : MonoBehaviour
{
    [SerializeField] private GameObject camFocus;
    [SerializeField] private GameObject camDown;
    [SerializeField] private GameObject player;


   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.S))
        {
            camFocus.transform.position = camDown.transform.position;
        }
        else
        {
            camFocus.transform.position = player.transform.position;
        }
    }
}
