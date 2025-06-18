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
            {                                                     //Todo: Should display the change every 0.5s
                Temperature = Mathf.Clamp(Temperature-2*Time.deltaTime/0.5f, MinHeat, MaxHeat);
                
            }
            else
            {
                Temperature = Mathf.Clamp(Temperature+Time.deltaTime/0.5f, MinHeat, MaxHeat);
            }

            if (Temperature >= 100)
            {
            print ("YOWZA"); 
            }
            else
            {
            print ("That bad bitch from Deltarune");
            }

            yield return null;
        }
    }
}
