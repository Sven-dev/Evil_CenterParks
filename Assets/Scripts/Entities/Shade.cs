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
            Animator.SetInteger("Frustration", Frustration);
            if (Frustration == 20)
            {
                Frustrated = true;
                OnHaunt?.Invoke();
                // Enable frustrated Visual Stage 5
            }

            yield return new WaitForSecondsRealtime(Cooldown);
            if (MovementOpportunity())
            {
                if (Frustration != 20)
                {
                    if (Office.RadioWorking == true)
                    {
                        //Radio turned on
                        //Frustration decreases by 2
                        Frustration = Mathf.Clamp(Frustration - 1, MinFrustration, MaxFrustration);
                    }
                    else
                    {
                        //Radio turned off
                        //Frustration increases by 2
                        Frustration = Mathf.Clamp(Frustration + 1, MinFrustration, MaxFrustration);
                    }
                }
            }
        }
    }
} 
