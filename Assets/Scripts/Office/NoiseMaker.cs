using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    [Header("Noise")]
    [SerializeField] private bool _MakingNoise = false;
    public bool MakingNoise
    {
        get { return _MakingNoise; }
        set
        {
            if (_MakingNoise != value)
            {
                _MakingNoise = value;
                if (_MakingNoise)
                {
                    NoiseManager.Instance.AlterNoiseFloor(+NoiseLevel);
                }
                else
                {
                    NoiseManager.Instance.AlterNoiseFloor(-NoiseLevel);
                }
            }
        }
    }
    [Range(0, 5)]
    [SerializeField] private int NoiseLevel;
}