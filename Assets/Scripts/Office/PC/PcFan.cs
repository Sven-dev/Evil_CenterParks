using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcFan : MonoBehaviour
{
    [SerializeField] private Perspective Perspective;
    [SerializeField] private AudioSource FanAudio;
    [SerializeField] private NoiseMaker NoiseMaker;

    private bool Powered = false;
    private bool Running = false;

    [Header("Temperature")]
    [SerializeField] private float Temperature;
    [Space]
    [SerializeField] private float MinimumTemperature = 55;
    [SerializeField] private float MaximumTemperature = 115;
    [Space]
    [SerializeField] private GameObject OverheatWarning;
    [SerializeField] private UnityIntEvent OnTemperatureUpdate;

    private void Awake()
    {
        StartCoroutine(_Temperature());
    }

    public void Update()
    {
        if (Perspective.Active && Powered && Input.GetKeyDown(KeyCode.F))
        {
            if (Running)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }

    public void TurnOn()
    {
        Running = true;
        NoiseMaker.MakingNoise = true;

        FanAudio.volume = 0.2f;
        FanAudio.pitch = 1f;        
    }

    public void TurnOff()
    {
        Running = false;
        NoiseMaker.MakingNoise = false;

        FanAudio.volume = 0.1f;
        FanAudio.pitch = 0.95f;
    }

    public void Power(bool state)
    {
        Powered = state;
        Running = Powered;

        TurnOff();
    }

    private IEnumerator _Temperature()
    {
        while (true)
        {
            if (Powered)
            {
                //Pc turned on
                if (Running)
                {
                    //Fan turned on, Temperature decreases by 1 per second
                    Temperature = Mathf.Clamp(Temperature - 1f * Time.deltaTime, MinimumTemperature, MaximumTemperature);
                }
                else
                {
                    //Fan turned off, Temperature increases by 3 per second
                    Temperature = Mathf.Clamp(Temperature + 3f * Time.deltaTime, MinimumTemperature, MaximumTemperature);              
                }
            }
            else
            {
                //Pc turned off, Temperature increases by 0.25 per second
                Temperature = Mathf.Clamp(Temperature + 0.25f * Time.deltaTime, MinimumTemperature, MaximumTemperature);
            }

            if (Temperature >= 90)
            {
                ParkCamera.Framerate = Mathf.Lerp(10, 0.666f, Temperature / MaximumTemperature);
                OverheatWarning.SetActive(true);
            }
            else
            {
                OverheatWarning.SetActive(false);
            }

            OnTemperatureUpdate?.Invoke((int)Temperature);
            yield return null;
        }
    }
}