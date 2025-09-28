using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.UI;

public class RadialTimeBar : MonoBehaviour
{
    public Timer timer;
    public float time { get { return timer.timeLeft; } }
    public float startTime = 60.0f;
    public bool timerStarted;
    
    public Image radialBar;

    public void Start()
    {
        timer.countDownTime = startTime;
    }
    public void Update()
    {
        if(!timerStarted)
            timer.StartTimer(startTime, false);
        if (timerStarted && startTime > 0)
        {
            float normalized = 1f - Mathf.Clamp01(timer.timeLeft / startTime);

            radialBar.fillAmount = normalized;
        }
    }

    public void StartTimer()
    {
        timer.StartTimer(startTime, false);
        timerStarted = true;
        radialBar.fillAmount = 0f;
    }
}
