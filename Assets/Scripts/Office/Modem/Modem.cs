using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modem : Powerable
{
    [Header("Modem")]
    [SerializeField] private float BreakChance;
    [Space]
    [SerializeField] private AudioSource BrokenAudio;
    [SerializeField] private AudioSource TuningAudio;
    [SerializeField] private NoiseMaker NoiseMaker;

    public static bool broken = false;

    private IEnumerator Start()
    {
        base.Start();
        while (true)
        {
            yield return new WaitForSecondsRealtime(30);
            if (HasPower && !broken)
            {
                float rnd = Random.Range(0, 1f);
                if (rnd < BreakChance)
                {
                    BreakModem();
                }
            }
        }
    }

    public void OnMouseDown()
    {
        if (broken)
        {          
            StartCoroutine("_ModemFixTimer");
            TuningAudio.volume = 0.45f;
        }
    }

    public void OnMouseUp()
    {
        StopCoroutine("_ModemFixTimer");
        TuningAudio.volume = 0f;
    }

    private IEnumerator _ModemFixTimer()
    {
        float time = 3;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        FixModem();
    }

    public void BreakModem()
    {
        if (!broken)
        {
            broken = true;
            BrokenAudio.volume = 0.6f;

            NoiseMaker.MakingNoise = true;
            UsingPower = true;
        }
    }

    public void FixModem()
    {
        broken = false;

        BrokenAudio.volume = 0f;
        TuningAudio.volume = 0f;
     
        NoiseMaker.MakingNoise = false;
        UsingPower = false;
    }

    protected override void LosePower()
    {
        base.LosePower();
        FixModem();
    }

    protected override void RegainPower()
    {
        base.RegainPower();
        BreakModem();
    }
}