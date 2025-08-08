using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GuestManager : MonoBehaviour
{
    [SerializeField] private List<GuestArrivalData> Guests;
    [Space]
    [SerializeField] private Transform DoorPivot;
    [SerializeField] private Transform ShutterPivot;
    [SerializeField] private Transform ParkPivot;
    [Space]
    [SerializeField] private UnityVoidEvent OnGuestAtDoor;

    private Guest ActiveGuest;

    public void CheckForGuestArrival(TimeSpan currentTime)
    {
        print("Time: " + currentTime.Hours + ":" + currentTime.Minutes);
        foreach (GuestArrivalData guest in Guests)
        {
            TimeSpan arrival = new TimeSpan(1, guest.ArrivalHour, guest.ArrivalMinute, 0);
            print("Guest arrival time: " + arrival.Hours + ":" + arrival.Minutes);

            if (!guest.Arrived && currentTime >= arrival)
            {               
                guest.Arrived = true;
                ActiveGuest = Instantiate(guest.Prefab, DoorPivot.position, Quaternion.Euler(0, 90, 0), transform);
                OnGuestAtDoor?.Invoke();
                print("A guest has arrived.");               
            }
        }
    }

    public void MoveGuestToShutter()
    {
        ActiveGuest.MoveTo(ShutterPivot);
    }

    public void MoveGuestToDoor()
    {
        ActiveGuest.MoveTo(DoorPivot);
        Invoke("UnloadGuest", 1f);
    }
    
    public void MoveGuestToPark()
    {
        ActiveGuest.MoveTo(ParkPivot);
        Invoke("UnloadGuest", 1f);

        //To do: Spawn guest park entity
    }

    private void UnloadGuest()
    {
        Destroy(ActiveGuest.gameObject);
    }
}

[System.Serializable]
public class GuestArrivalData
{
    [Range(0, 6)]
    public int ArrivalHour;
    [Range(0, 59)]
    public int ArrivalMinute;
    public bool ValidGuest = true;
    [Space]
    public Guest Prefab;
    [HideInInspector] public bool Arrived = false;
}