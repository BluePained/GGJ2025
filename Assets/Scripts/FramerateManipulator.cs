using UnityEngine;

public class FramerateManipulator : MonoBehaviour
{
    [SerializeField] private int framerateNum;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = framerateNum;
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = framerateNum;
    }
}
