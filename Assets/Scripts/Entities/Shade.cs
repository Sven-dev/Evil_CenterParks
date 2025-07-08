using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : Entity
{
    [SerializeField] private Animator Animator;
    [SerializeField] private int Frustration;
    [Space]
    [SerializeField] private int MinFrustration = 0;
    [SerializeField] private int MaxFrustration = 20;
    [Space]
    [SerializeField] private UnityVoidEvent OnHaunt;
    public OfficeManager Office;
    public bool Frustrated = false;


    private IEnumerator _BehaviourLoop()
    { 
        yield return null;
        while (Frustrated == false)
        {
            Animator.SetInteger("Frustration", Frustration);
            yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);
            if (MovementOpportunity())
            {
                if (Frustration == 20)
                {
                    Frustrated = true;
                    OnHaunt?.Invoke();
                    // Enable frustrated Visual Stage 5
                }
                if (Frustration != 20)
                {
                    if (Office.RadioWorking == true)
                    {
                        //Radio turned on
                        //Frustration decreases by 2
                        Frustration = Mathf.Clamp(Frustration - 2, MinFrustration, MaxFrustration);
                    }
                    else
                    {
                        //Radio turned off
                        //Frustration increases by 2
                        Frustration = Mathf.Clamp(Frustration + 2, MinFrustration, MaxFrustration);
                    }
                }
            }
        }
    }
} 
