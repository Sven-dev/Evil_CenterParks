using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCBooter : MonoBehaviour
{
    [SerializeField] private Perspective Perspective;
    [SerializeField] private OfficeManager Office;
    [SerializeField] private PcFan Fan;
    [Space]
    [SerializeField] private UnityVoidEvent OnPCBootSound;
    [SerializeField] private UnityVoidEvent OnPCBootup;
    [SerializeField] private UnityVoidEvent OnPCBootdown;

    [SerializeField] private UnityVoidEvent OnPCBootCancel;

    private bool Booting = false;

    public void Update()
    {
        if (Perspective.Active && Office.PowerWorking)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                TogglePc();
            }
        }
    }

    public void Start()
    {
        StartCoroutine("_StartBootDelay");  
    }

    public void TogglePc()
    {
        //If the pc is on, turn the pc & fan off
        if (Office.PcRunning)
        {
            OnPCBootdown?.Invoke();
            if (Office.FanRunning)
            {
                Fan.TurnOff();
            }
        }
        //If the pc is off and not booting, start the boot sequence
        else if (!Booting)
        {
            StartCoroutine("_StartBootDelay");
        }
    }

    public void TurnOffPc()
    {
        OnPCBootdown?.Invoke();
        Fan.TurnOff();
    }

    private IEnumerator _StartBootDelay()
    {
        //Starts boot, waiting a set period before turning the pc & fan back on
        Booting = true;
        OnPCBootSound?.Invoke();
        float BootTime = 3;
        while (BootTime > 0)
        {
            BootTime -= Time.deltaTime;
            yield return null;

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Booting = false;
                OnPCBootCancel?.Invoke();
                StopCoroutine("_StartBootDelay");
            }
        }

        OnPCBootup?.Invoke();
        Fan.TurnOn();
        Booting = false;
    }
}