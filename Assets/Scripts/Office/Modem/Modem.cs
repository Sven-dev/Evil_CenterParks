using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modem : MonoBehaviour
{
    [SerializeField] private int ActivePerspective;
    [Space]
    [SerializeField] private float BreakChance;
    [SerializeField] private Button ResetButton;
    [Space]
    [SerializeField] private UnityVoidEvent OnModemBreak;
    [SerializeField] private UnityVoidEvent OnModemReset;

    private void Start()
    {
        StartCoroutine(_RandomBreak());
    }

    public void ToggleActive(int cameraPerspective)
    {
        if (cameraPerspective == ActivePerspective)
        {
            ResetButton.interactable = true;
        }
        else
        {
            ResetButton.interactable = false;
        }
    }

    public void StartReset()
    {
        StartCoroutine("_ResetTimer");
    }

    public void StopReset()
    {
        StopCoroutine("_ResetTimer");
    }

    public void BreakModem()
    {
        OnModemBreak?.Invoke();
    }

    private IEnumerator _ResetTimer()
    {
        float time = 5;
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
