using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcFan : MonoBehaviour
{
    [SerializeField] private Perspective Perspective;
    [SerializeField] private OfficeManager Office;

    [Space]
    [SerializeField] private UnityVoidEvent OnFanEnable;
    [SerializeField] private UnityVoidEvent OnFanDisable;

    public void Update()
    {
        if (Perspective.Active)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleFan();
            }
        }
    }

    public void ToggleFan()
    {
        if (Office.PcRunning)
        {
            if (Office.FanRunning)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }

    public void TurnOn()
    {
        Office.FanRunning = true;
        OnFanEnable?.Invoke();            
    }

    public void TurnOff()
    {
        Office.FanRunning = false;
        OnFanDisable?.Invoke();
    }
}
