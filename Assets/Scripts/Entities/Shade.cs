using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : Entity
{
    [SerializeField] private Perspective Perspective;
    [SerializeField] private Animator Animator;
    private int Frustration;
    [Space]
    private int MinFrustration = 0;
    private int MaxFrustration = 20;
    [Space]
    [SerializeField] private UnityVoidEvent OnHaunt;

    public OfficeManager Office;
    public bool Frustrated = false;

    private IEnumerator _BehaviourLoop()
    {
        yield return null;
        while (Frustrated == false)
        {
            yield return new WaitForSecondsRealtime(Cooldown);
            if (MovementOpportunity())
            {
                if (Office.RadioWorking == true)
                {
                    //Radio turned on
                    //Frustration decreases by 1
                    Frustration = Mathf.Clamp(Frustration - 1, MinFrustration, MaxFrustration);
                }
                else
                {
                    //Radio turned off
                    //Frustration increases by 1
                    Frustration = Mathf.Clamp(Frustration + 1, MinFrustration, MaxFrustration);
                    if (Frustration == MaxFrustration)
                    {
                        Frustrated = true;
                        OnHaunt?.Invoke();
                        //Enable frustrated Visual Stage 5
                    }
                }

                Animator.SetInteger("Frustration", Frustration);              
            }
        }
    }
} 
