using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    [SerializeField] private UnityVoidEvent OnEntityUpdate;
    public OfficeManager Office;

    [Header("Power")]
    [Range(0, 4)]
    [SerializeField] private int PowerLevel = 0;

    private const int PowerCeiling = 4;

    [Space]
    [SerializeField] private UnityFloatEvent OnPowerLevelChange;
    [SerializeField] private UnityVoidEvent OnBlackout;
    [SerializeField] private UnityVoidEvent OnBreaker;

    [SerializeField] private bool Blackout = false;

    private void Start()
    {
        OnPowerLevelChange?.Invoke(PowerLevel);
    }

    public int GetPowerLevel()
    {
        return PowerLevel;
    }

    public void AlterPowerLevel(int amount)
    {
        if (PowerLevel + amount >= 4)
        {
            OnBlackout?.Invoke();
            Blackout = true;
            Office.PowerWorking = false;
        }
        else
        {
            PowerLevel = Mathf.Clamp(PowerLevel + amount, 0, PowerCeiling);
            OnPowerLevelChange?.Invoke(PowerLevel);
        }
    }

    public void OnMouseDown()
    {
        if (Blackout == true)
        {
            OnBreaker?.Invoke();
            Blackout = false;
            Office.PowerWorking = true;
        }
    }
}
