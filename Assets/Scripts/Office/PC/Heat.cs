using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour
{
    [SerializeField] private float Temperature;
    [SerializeField] private UnityVoidEvent Overheating;
    [SerializeField] private GameObject Static;
    float MinHeat = 55;
    float MaxHeat = 115;
    public OfficeManager Office;
    private void Start()
    {
    StartCoroutine(TempC());
    }    
    private IEnumerator TempC()
    {
    while (true)    
    {
    if (Office.PCFan == false)
    {
        Temperature = Mathf.Clamp(Temperature+Time.deltaTime*2, MinHeat, MaxHeat);
        yield return null;
    }
    else
    {
        Temperature = Mathf.Clamp(Temperature-2*Time.deltaTime*2, MinHeat, MaxHeat);
        yield return null;
    }
    if (Temperature>=100)
       {
        Static.SetActive(true);
        yield return null;
        }
    if (Temperature<=100)
       {

        }      
    }
    }
    }
