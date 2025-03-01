using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField] private Text Label;

    public void UpdateTime(TimeSpan time)
    {
        Label.text = string.Format("{0:00}:{1:00}", time.Hours, time.Minutes);
    }
}