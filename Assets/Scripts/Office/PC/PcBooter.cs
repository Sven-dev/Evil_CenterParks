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
        if (Perspective.Active)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                TogglePc();
            }
        }
    }

    public void TogglePc()
    {
        if (Office.PcRunning == true)
        {
            OnPCBootdown?.Invoke();
            if (Office.FanRunning == true)
            {
                Fan.TurnOff();
            }
        }
        else if (Booting == false)
        {
            StartCoroutine("_StartBootDelay");          
        }
    }

    private IEnumerator _StartBootDelay()
    {
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