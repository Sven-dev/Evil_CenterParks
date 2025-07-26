using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : Powerable
{
    [Space]
    [SerializeField] private AudioSource Music;
    [SerializeField] private NoiseMaker NoiseMaker;

    public bool On = false;
    public bool Haunted = false;

    public void OnMouseDown()
    {
        if (HasPower && !Haunted) 
        {
            if (On)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }

    private void TurnOn()
    {
        Music.volume = 0.6f;
        NoiseMaker.MakingNoise = true;
        UsingPower = true;
    }

    private void TurnOff()
    {
        Music.volume = 0;
        NoiseMaker.MakingNoise = false;
        UsingPower = false;
    }

    public void Haunt()
    {
        Haunted = true;
        TurnOn();
    }

    protected override void LosePower()
    {
        base.LosePower();
        TurnOff();
    }

    protected override void RegainPower()
    {
        base.RegainPower();
        if (Haunted)
        {
            TurnOn();
        }
    }
}