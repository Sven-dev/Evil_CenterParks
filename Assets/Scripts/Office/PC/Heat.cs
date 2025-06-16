using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour
{
    [SerializeField] private float Temperature;
    [SerializeField] private float MinHeat = 55;
    [SerializeField] private float MaxHeat = 115;
    [Space]
    [SerializeField] private GameObject Static;
    [SerializeField] private OfficeManager Office;
    [Space]
    [SerializeField] private UnityVoidEvent Overheating;

    private void Start()
    {
        StartCoroutine(TempC());
    }

    private IEnumerator TempC()
    {
        while (true)
        {
            if (Office.PCFan == true)
            {
                Temperature = Mathf.Clamp(Temperature - 2 * Time.deltaTime * 2, MinHeat, MaxHeat);
                
            }
            else
            {
                Temperature = Mathf.Clamp(Temperature + Time.deltaTime * 2, MinHeat, MaxHeat);
            }

            if (Temperature >= 100)
            {

            }
            else
            {

            }

            yield return null;
        }
    }
}
