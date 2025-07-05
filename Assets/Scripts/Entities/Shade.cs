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
            yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);
            if (MovementOpportunity())
            {
                if (Frustration == 20)
                {
                    Frustrated = true;
                    OnHaunt?.Invoke();
                    // Enable frustrated Visual Stage 5
                    CurrentRoom.LeaveRoom(EntityType);
                    CurrentRoom = RoomController.Instance.GetRoom(1);
                    CurrentRoom.EnterRoom(EntityType);
                }
                else if (Frustration >= 16)
                {
                    // Enable frustrated Visual Stage 4
                    CurrentRoom.LeaveRoom(EntityType);
                    CurrentRoom = RoomController.Instance.GetRoom(2);
                    CurrentRoom.EnterRoom(EntityType);
                }
                else if (Frustration >= 12)
                {
                    // Enable frustrated Visual Stage 3
                    CurrentRoom.LeaveRoom(EntityType);
                    CurrentRoom = RoomController.Instance.GetRoom(3);
                    CurrentRoom.EnterRoom(EntityType);
                }
                else if (Frustration >= 8)
                {
                    // Enable frustrated Visual Stage 2
                    CurrentRoom.LeaveRoom(EntityType);
                    CurrentRoom = RoomController.Instance.GetRoom(4);
                    CurrentRoom.EnterRoom(EntityType);
                }
                else if (Frustration >= 4)
                {
                    // Enable frustrated Visual Stage 1
                    CurrentRoom.LeaveRoom(EntityType);
                    CurrentRoom = RoomController.Instance.GetRoom(5);
                    CurrentRoom.EnterRoom(EntityType);
                }
                else if (Frustration == 0)
                {
                    // Enable frustrated Visual Stage 0
                    CurrentRoom.LeaveRoom(EntityType);
                    CurrentRoom = RoomController.Instance.GetRoom(6);
                    CurrentRoom.EnterRoom(EntityType);
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
                        //Frustration increases by 1
                        Frustration = Mathf.Clamp(Frustration + 2, MinFrustration, MaxFrustration);
                    }
                }
            }
        } 
    }
} 
