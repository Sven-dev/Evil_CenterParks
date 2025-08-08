using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoorButton : MonoBehaviour
{
    [SerializeField] private bool Active = true;

    [SerializeField] private UnityVoidEvent OnDoorOpened;
    [SerializeField] private UnityVoidEvent OnGuestDenied;

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
                OnDoorOpened?.Invoke();
            }
            else if (GuestCheck)
            {
                GuestCheck = false;
                OnGuestDenied?.Invoke();
            }
        }
    }

    public void ReadyDoor()
    {
        DoorRequiresOpening = true;
    }
}