using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerable : MonoBehaviour
{
    [Header("Power")]
    [Range(0, 5)]
    [SerializeField] private int PowerUsage;
    [SerializeField] protected bool HasPower = true;
    [SerializeField] private bool _UsingPower = false;
    protected bool UsingPower
    { 
        get { return _UsingPower; } 
        set 
        {
            if (_UsingPower != value)
            {
                _UsingPower = value;
                if (_UsingPower)
                {
                    PowerManager.Instance.AlterPowerLevel(+PowerUsage);
                }
                else
                {
                    PowerManager.Instance.AlterPowerLevel(-PowerUsage);
                }
            }
        }
    }

    protected virtual void Start()
    {
        PowerManager.Instance.OnBlackout.AddListener(LosePower);
        PowerManager.Instance.OnRestorePower.AddListener(RegainPower);
    }

    protected virtual void OnDestroy()
    {
        PowerManager.Instance.OnBlackout.RemoveListener(LosePower);
        PowerManager.Instance.OnRestorePower.RemoveListener(RegainPower);
    }

    protected virtual void LosePower()
    {
        HasPower = false;
        if (UsingPower)
        {
            UsingPower = false;
        }
    }

    protected virtual void RegainPower()
    {
        HasPower = true;
    }
}