using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the temperature of the pc. Pc can be cooled down using the fan, and will cause glitching effects on the camera when overheating.
/// </summary>
public class Heat : MonoBehaviour
{
    [SerializeField] private float Temperature;
    [Space]
    [SerializeField] private float MinHeat = 55;
    [SerializeField] private float MaxHeat = 115;
    [Space]
    [SerializeField] private OfficeManager Office;
    [Space]
    // Todo: This might need to be 1 event that take the temperature into account, so connected scripts can do things based on how overheated the things is.
    [SerializeField] private UnityVoidEvent OnOverheat;
    [SerializeField] private UnityVoidEvent OnCooledDown;

    private bool Overheating = false;

    private void Start()
    {
        StartCoroutine(_ManageTemperature());
    }

    private IEnumerator _ManageTemperature()
    {
        while (true)
        {
            if (Office.FanRunning)
            {
                //Fan turned on
                //Temperature decreases by 0.75 (decrement value * wait time)
                Temperature = Mathf.Clamp(Temperature - 1f * 0.5f, MinHeat, MaxHeat);
            }
            else if (Office.PcRunning == false)
            {
                //Pc turned off
                //Temperature decreases by 1 (decrement value * wait time)
                Temperature = Mathf.Clamp(Temperature - 0.25f * 0.5f, MinHeat, MaxHeat);
            }            
            else if (Office.PcRunning == true)
            {
                //Fan turned off
                //Temperature increases by 0.5 (increment value * wait time)
                Temperature = Mathf.Clamp(Temperature + 3 * 0.5f, MinHeat, MaxHeat);
            }

            if (!Overheating)
            {
                if (Temperature >= 100)
                {
                    Overheating = true;
                    OnOverheat?.Invoke();

                    print("Pc overheating!");
                }
            }
            else
            {
                if (Temperature < 100)
                {
                    Overheating = false;
                    OnCooledDown?.Invoke();

                    print("Pc cooled down...");
                }
            }

            //Wait 0.5 seconds
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
