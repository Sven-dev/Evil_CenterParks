using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkDoorButton : MonoBehaviour
{
    [SerializeField] private bool Active = true;

    [SerializeField] private UnityVoidEvent OnGuestAccepted;

    private bool GuestCheck = false;

    private void OnMouseDown()
    {
        if (Active)
        {
            if (GuestCheck)
            {
                GuestCheck = false;
                OnGuestAccepted?.Invoke();
            }
        }
    }

    public void ReadyDoor()
    {
        GuestCheck = true;
    }
}
