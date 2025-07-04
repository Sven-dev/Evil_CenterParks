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
        if (Shadow.Frustrated == false)
        {
            if (Office.RadioWorking == true)
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
