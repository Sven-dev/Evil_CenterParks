using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modem : MonoBehaviour
{
    [SerializeField] private float BreakChance;
    [SerializeField] private Button ResetButton;
    [Space]
    [SerializeField] private UnityVoidEvent OnModemBreak;
    [SerializeField] private UnityVoidEvent OnModemReset;
    public OfficeManager Office;
    public Shade Shadow;

    private void Start()
    {
        StartCoroutine(_RandomBreak());
    }

    public void Interactable(bool state)
    {
        if (Shadow.Frustrated == true)
        {
            {
                ResetButton.interactable = false;
            }
        }
        else
        {
            if (state == true && !Office.ModemWorking)
            {
                ResetButton.interactable = true;
            }
            else
            {
                ResetButton.interactable = false;
            }
        }
    }

    public void OnMouseDown()
    {
        if (RoomController.Instance.GetRoom(1) == RoomController.Instance.FindEntity(EntityType.Vial))
        {
            StartCoroutine("_ResetTimer");
        }
        else if (Office.ModemWorking == true)
        {
            OnModemBreak?.Invoke();
        }
        else
        {
            OnModemReset?.Invoke();
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
