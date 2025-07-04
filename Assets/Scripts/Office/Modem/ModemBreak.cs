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
    public OfficeManager Office;
    private void Start()
    {
        StartCoroutine(_RandomBreak());
    }

    public void OnMouseDown()
    {
        if (Office.ModemWorking == false)
        {
            StartCoroutine("_ResetTimer");
        }
    }

    public void OnMouseUp()
    {
        StopCoroutine("_ResetTimer");
    }

    public void BreakModem()
    {
        OnModemBreak?.Invoke();
    }

    private IEnumerator _ResetTimer()
    {
        float time = 3;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

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
