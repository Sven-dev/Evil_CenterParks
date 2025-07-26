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

    [SerializeField] private Radio Radio;
    public bool Frustrated = false;

    private IEnumerator _BehaviourLoop()
    {
        yield return null;
        while (Frustrated == false)
        {
            yield return new WaitForSecondsRealtime(Cooldown);
            if (MovementOpportunity())
            {
                if (Radio.On)
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
                        Radio.Haunt();
                        OnHaunt?.Invoke();
                        //Enable frustrated Visual Stage 5

                        StopCoroutine("_BehaviourLoop");                      
                    }
                }

                Animator.SetInteger("Frustration", Frustration);              
            }
        }
    }
} 