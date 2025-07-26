using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance;

    [Header("Power")]
    [SerializeField] private int PowerLevel = 0;
    [SerializeField] private int PowerCeiling = 4;
    [Space]
    [SerializeField] private UnityFloatEvent OnPowerLevelChange = new UnityFloatEvent();
    [SerializeField] public UnityVoidEvent OnBlackout = new UnityVoidEvent();
    [SerializeField] public UnityVoidEvent OnRestorePower = new UnityVoidEvent();

    private bool Blackout = false;

    private void Awake()
    {
        Instance = this;
    }

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
        if (PowerLevel + amount >= PowerCeiling)
        {
            OnBlackout?.Invoke();
            Blackout = true;
        }
        else
        {
            PowerLevel = Mathf.Clamp(PowerLevel + amount, 0, PowerCeiling);
            OnPowerLevelChange?.Invoke(PowerLevel);
        }
    }

    public void OnMouseDown()
    {
        if (Blackout)
        {
            OnRestorePower?.Invoke();
            Blackout = false;
        }
    }
}