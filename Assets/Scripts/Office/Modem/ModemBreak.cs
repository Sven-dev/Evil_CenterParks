using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modem : MonoBehaviour
{
    [SerializeField] private float BreakChance;
    [Space]
    [SerializeField] private UnityVoidEvent OnModemBreak;
    [SerializeField] private UnityVoidEvent OnModemReset;
    [SerializeField] private UnityVoidEvent OnStartTune;
    [SerializeField] private UnityVoidEvent OnStopTune;
    public OfficeManager Office;
    private void Start()
    {
        StartCoroutine(_RandomBreak());
    }

    public void OnMouseDown()
    {
        //Prevents "fixing" a modem that isn't broken
        if (!Office.ModemWorking && Office.PowerWorking)
        {
            StartCoroutine("_ResetTimer");
        }
    }

    public void OnMouseUp()
    {
        StopCoroutine("_ResetTimer");
        OnStopTune?.Invoke();
    }

    public void BreakModem()
    {
        if (Office.ModemWorking && Office.PowerWorking)
        {
            OnModemBreak?.Invoke();
        }
    }

    private IEnumerator _ResetTimer()
    {
        OnStartTune?.Invoke();
        float time = 3;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        FixModem();
    }

    public void FixModem()
    {
        OnModemReset?.Invoke();
    }

    private IEnumerator _RandomBreak()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(30);
            float rnd = Random.Range(0, 1f);
            if (rnd < BreakChance)
            {
                BreakModem();
            }
        }
    } 
}
