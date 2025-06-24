using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : Entity
{
    [SerializeField] private int Frustration;
    [Space]
    [SerializeField] private int MinFrustration = 0;
    [SerializeField] private int MaxFrustration = 20;
    [Space]
    [SerializeField] private UnityVoidEvent OnHaunt;
    public OfficeManager Office;
    public bool Frustrated = false;

 /*   private void Start()
    {
        StartCoroutine(_ManageFrustration());
    }*/

    private IEnumerator _BehaviourLoop()
    {
        yield return null;
        while (Frustrated == false)
        {
            if (MovementOpportunity())
            {
                if (Frustration == 20)
                {
                    Frustrated = true;
                    OnHaunt?.Invoke();

                    print("Shade Frustrated!");
                }
                else if (Office.ModemWorking == false)
                {
                    //Radio turned on
                    //Frustration decreases by 2
                    Frustration = Mathf.Clamp(Frustration - 2, MinFrustration, MaxFrustration);
                }
                else
                {
                    //Radio turned off
                    //Frustration increases by 1
                    Frustration = Mathf.Clamp(Frustration + 1, MinFrustration, MaxFrustration);
                }
                yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);
            }
        } 
    }
} 
