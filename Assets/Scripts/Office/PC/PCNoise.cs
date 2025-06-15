using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCNoise : MonoBehaviour
{
    [SerializeField] private int ActivePerspective;
    [Space]
    [SerializeField] private Button PowerButton;
    [Space]
    [SerializeField] private UnityVoidEvent OnPCBootdown;
    [SerializeField] private UnityVoidEvent OnPCBootup;
    [SerializeField] private UnityVoidEvent OnPCFanoff;
    [SerializeField] private UnityVoidEvent OnPCFanon;
    private bool PCBootDelay = false;
    public OfficeManager Office;

    public void ToggleActive(int cameraPerspective)
    {
        if (cameraPerspective == ActivePerspective)
        {
            PowerButton.interactable = true;
        }
        else
        {
            PowerButton.interactable = false;
        }
    }

    public void Update()
    {
        //if PCWorking == False
        //and StartBoot
        //then  StartBootDelay
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartBoot();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartFan();
        }
    }

    private IEnumerator StartBootDelay()
    {
        PCBootDelay = true;
        float BootTime = 3;
        while (BootTime > 0)
        {
            BootTime -= Time.deltaTime;
            yield return null;

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                PCBootDelay = false;
                StopCoroutine("StartBootDelay");
            }
        }

        OnPCBootup?.Invoke();
        OnPCFanon?.Invoke();
        PCBootDelay = false;
    }

    public void StartBoot()
    {
        if (Office.PCWorking == true)
        {
            OnPCBootdown?.Invoke();
            if (Office.PCFan == true)
            {
                OnPCFanoff?.Invoke();
            }
        }
        else if (PCBootDelay == false)
        {
            StartCoroutine("StartBootDelay");          
        }
    }

    public void StartFan()
    {
        if (Office.PCWorking == true)
        {
            if (Office.PCFan == true)
            {
                OnPCFanoff?.Invoke();
            }
            else
            {
                OnPCFanon?.Invoke();
            }
        }
    }
}