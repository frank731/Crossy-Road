using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimerController : MonoBehaviour
{
    public float startTime = 10f;
    public UnityEvent timerFinished;

    private Image timerBar;
    private float timeLeft = 0;
    private bool started = false;

    public void StartTimer()
    {
        timeLeft = startTime;
        started = true;
    }

    private void Start()
    {
        timerBar = GetComponent<Image>();
        //timeLeft = startTime;
    }

    private void Update()
    {
        if (started)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = timeLeft / startTime;
            }
            else
            {
                started = false;
                timerFinished.Invoke();
            }
        }
    }
}
