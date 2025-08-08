using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightProgressTimer : MonoBehaviour
{
    [Tooltip("The amount of seconds an in-game hour lasts for.")]
    [SerializeField] private float HourDuration = 90f; 
    [Space]
    [SerializeField] private UnityTimeSpanEvent OnHourPassed;
    [SerializeField] private UnityTimeSpanEvent OnMinutePassed;
    [SerializeField] private UnityVoidEvent OnNightFinished;

    private TimeSpan currentTime = new TimeSpan(23, 0, 0);
    private TimeSpan endTime = new TimeSpan(1, 7, 0, 0);

    private TimeSpan CustomTimeScale;

    private void Start()
    {
        CustomTimeScale = new TimeSpan(0, 0, (int)(60 * 60 / HourDuration));
        //print(CustomTimeScale);
        OnHourPassed?.Invoke(currentTime);
        StartCoroutine(_NightTimer());
    }

    private IEnumerator _NightTimer()
    {
        int currentHour = currentTime.Hours;
        while (currentTime < endTime)
        {          
            currentTime += CustomTimeScale;
            OnMinutePassed?.Invoke(currentTime);
            //print("Time: " + currentTime.Hours + ":" + currentTime.Minutes + "." + currentTime.Seconds);
         
            if (currentTime.Hours != currentHour)
            {
                currentHour = currentTime.Hours;
                print("An hour has passed");
                OnHourPassed?.Invoke(currentTime);
            }

            yield return new WaitForSecondsRealtime(1);
        }

        print("Night over");
        OnNightFinished?.Invoke();
    }
}