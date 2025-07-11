using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    [SerializeField] private UnityVoidEvent OnRadioOn;
    [SerializeField] private UnityVoidEvent OnRadioOff;
    public OfficeManager Office;
    public Shade Shadow;

    public void OnMouseDown()
    {
        if (!Shadow.Frustrated) 
        {
            //Turns radio on or off
            if (Office.RadioWorking)
            {
                OnRadioOff?.Invoke();
            }
            else
            {
                OnRadioOn?.Invoke();
            }
        }
    }
}
