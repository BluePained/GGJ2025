using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;

    private float timer;
    private float min;
    private float sec;
    private float millisec;
    private bool isRunning = false;

    // Update is called once per frame
    void Update()
    {
        string timeFormat = string.Format("{0:00}:{1:00}:{2:000}", min, sec, millisec);
        if (Input.anyKey)
        {
            isRunning = true;
        }

        if(isRunning)
        {
            timer += Time.deltaTime;

            min = Mathf.Floor(timer / 60f);
            sec = Mathf.Floor(timer % 60);
            millisec = (timer % 1) * 100;

            _timerText.text = timeFormat;
        }
        
    }
}
