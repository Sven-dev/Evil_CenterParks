using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoorButton : MonoBehaviour
{
    [SerializeField] private bool Active = true;

    [SerializeField] private UnityVoidEvent OnDoorOpened;
    [SerializeField] private UnityVoidEvent OnGuestDenied;
    [Space]
    [SerializeField] private UnityVoidEvent OnCorkKill;

    private bool DoorRequiresOpening = false;
    private bool GuestCheck = false;

    private void OnMouseDown()
    {
        if (Active)
        {
            if (DoorRequiresOpening)
            {
                DoorRequiresOpening = false;
                GuestCheck = true;
                GuestSystem.GuestManager.Instance.GuestAtDoor = false;
                OnDoorOpened?.Invoke();
            }
            else if (GuestCheck)
            {
                GuestCheck = false;
                OnGuestDenied?.Invoke();
            }
            else if (RoomController.Instance.FindEntity(EntityType.Cork).ID == 0)
            {
                OnCorkKill.Invoke();
            }
        }
    }

    public void ReadyDoor()
    {
        DoorRequiresOpening = true;
    }
}