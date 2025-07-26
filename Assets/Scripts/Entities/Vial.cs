using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vial : Entity
{
    private List<Room> IgnoredRooms = new List<Room>();
    [Space]    
    [SerializeField] private UnityVoidEvent OnEnteringOffice;
    [SerializeField] private UnityVoidEvent OnLeavingOfOffice;

    private bool InOffice = false;

    protected IEnumerator _BehaviourLoop()
    {
        yield return null;

        CurrentRoom.EnterRoom(EntityType);
        Log("AI enabled in " + CurrentRoom.name + ".");

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom, IgnoredRooms);
        Log("Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
        while (true)
        {
            if (CurrentRoom.ID == 1)
            {
                if (!InOffice)
                {                 
                    //If the shutter is closed when Vial enters room 1, instantly forces him out, otherwise he will enter the office
                    if (!Shutter.Open)
                    {
                        KickOutOfOffice();
                        continue;
                    }
                    else
                    {
                        InOffice = true;
                        OnEnteringOffice?.Invoke();
                        Log("Vial is in the office, and will not move until the modem is fixed.");
                        continue;
                    }
                }
                else if (Modem.broken)
                {           
                    yield return null;
                    continue;
                }
                else
                {
                    KickOutOfOffice();
                    OnLeavingOfOffice?.Invoke();
                    continue;
                }
            }

            if (CurrentRoom == currentRoute.Destination && CurrentRoom.GetNoiseLevel() == 5)
            {
                //Recalculate path
                int rnd = UnityEngine.Random.Range(1, 5);
                if (rnd == 1)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetCameraRoomPath(CurrentRoom);
                    Log("Beelining it from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
                else if (rnd >= 2)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom, IgnoredRooms);
                    Log("Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
            }

            Log("Waiting for next movement opportunity (" + Cooldown + " seconds)");
            yield return new WaitForSecondsRealtime(Cooldown);

            //Movement opportunites (Vial takes 2)
            for (int i = 1; i < 3; i++)
            {
                if (MovementOpportunity(21 * i))
                {
                    if (CurrentRoom != currentRoute.Destination)
                    {
                        //Walking
                        WalkToNextRoom();
                        Log("Went to " + CurrentRoom.name + ".");
                    }
                    else if (CurrentRoom.GetNoiseLevel() < 5)
                    {
                        //Noise raising
                        CurrentRoom.AlterNoiseLevel(+1);
                        Log("Raising " + CurrentRoom.name + " noise level to " + CurrentRoom.GetNoiseLevel() + ".");
                    }
                }

                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }

    public void KickOutOfOffice() //KickVialOut
    {
        CurrentRoom.LeaveRoom(EntityType);
        int rnd = UnityEngine.Random.Range(0, 3);
        if (rnd == 0)
        {
            CurrentRoom = RoomController.Instance.GetRoom(3);
        }
        else if (rnd == 1)
        {
            CurrentRoom = RoomController.Instance.GetRoom(8);
        }
        else if (rnd == 2)
        {
            CurrentRoom = RoomController.Instance.GetRoom(12);
        }

        CurrentRoom.EnterRoom(EntityType);
        routeProgress = 0;
        currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom, IgnoredRooms);
        Log("Got moved to " + CurrentRoom.name);
    }

    public void AddIgnoredRoom(Room room)
    {
        IgnoredRooms.Add(room);
    }
}