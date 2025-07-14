using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cork : Entity
{
    [SerializeField] private Entity Hallucination;

    private List<Room> IgnoredRooms = new List<Room>();

    protected IEnumerator _BehaviourLoop()
    {
        yield return null;
        CurrentRoom.EnterRoom(EntityType);
        Log("AI enabled in " + CurrentRoom.name + ".");

        //Hallucination setup
        bool hallucinationMode = false;
        float rnd = UnityEngine.Random.Range(60, 121);
        TimeSpan hallucinationTimer = DateTime.Now.TimeOfDay + TimeSpan.FromSeconds(UnityEngine.Random.Range(60, 121));
        Log("Setting a hallucination timer for " + rnd + " seconds.");

        IgnoredRooms.Add(CurrentRoom);
        routeProgress = 0;
        currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, IgnoredRooms);
        IgnoredRooms.Remove(CurrentRoom);
        Log("Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);

        while (true)
        {
            //Checking ignored rooms
            for (int i = IgnoredRooms.Count - 1; i >= 0; i--)
            {
                Room room = IgnoredRooms[i];
                if (room.GetNoiseLevel() == 0)
                {
                    IgnoredRooms.Remove(room);
                }
            }

            List<Room> loudestRooms = RoomController.Instance.GetLoudestRooms(IgnoredRooms);
            if (hallucinationMode)
            {
                if (CurrentRoom == currentRoute.Destination)
                {
                    //Clear hallucination
                    Hallucination.gameObject.SetActive(false);

                    //Generate new timer
                    rnd = UnityEngine.Random.Range(60, 121);
                    hallucinationTimer = DateTime.Now.TimeOfDay + TimeSpan.FromSeconds(rnd);

                    hallucinationMode = false;
                    Log("Hallucination mode done! setting a new hallucination timer for " + rnd + " seconds, and resuming normal behaviour.");
                }
            }
            else if (DateTime.Now.TimeOfDay > hallucinationTimer && CurrentRoom.ID != 1)
            {
                if (CurrentRoom.GetNoiseLevel() == 5 || loudestRooms[0].GetNoiseLevel() < 4)
                {
                    hallucinationMode = true;

                    //Create hallucination
                    Hallucination.gameObject.SetActive(true);

                    //Path to room 1 or 12, whichever is closer
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetRouteTo1or12(CurrentRoom);

                    Log("Hallucination mode: Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
            }

            //Waiting based on loudest noise level
            switch (loudestRooms[0].GetNoiseLevel())
            {
                case 5:
                    Log("Waiting for next movement opportunity (" + (Cooldown * 0.41f).ToString("F2") + " seconds)");
                    yield return new WaitForSecondsRealtime(Cooldown * 0.41f);
                    break;
                case 4:
                    Log("Waiting for next movement opportunity (" + (Cooldown * 0.82f).ToString("F2") + " seconds)");
                    yield return new WaitForSecondsRealtime(Cooldown * 0.82f);
                    break;
                default:
                    Log("Waiting for next movement opportunity (" + Cooldown + " seconds)");
                    yield return new WaitForSecondsRealtime(Cooldown);
                    break;
            }

            if (!hallucinationMode)
            {
                //if the destination room isn't one of the loudest rooms anymore, recalculate
                loudestRooms = RoomController.Instance.GetLoudestRooms(IgnoredRooms);
                if (loudestRooms[0].GetNoiseLevel() != currentRoute.Destination.GetNoiseLevel())
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, IgnoredRooms);

                    Log("Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
            }

            if (MovementOpportunity())
            {
                GuestRoomManager guestRooms = CurrentRoom.GuestRooms;
                OfficeManager office = CurrentRoom.Office;
                if (guestRooms && !guestRooms.Killed)
                {
                    if (guestRooms.BeingDisturbed)
                    {
                        guestRooms.Kill();
                        Log("Room " + CurrentRoom + " kill attempt successful.");
                        continue;
                    }
                    else
                    {
                        IgnoredRooms.Add(CurrentRoom);
                        Log("Room " + CurrentRoom + " kill attempt failed.");
                    }
                }
                else if (CurrentRoom.ID == 12)
                {
                    CurrentRoom.LeaveRoom(EntityType);
                    yield return new WaitForSecondsRealtime(3);

                    CurrentRoom = RoomController.Instance.GetRoom(1);
                    CurrentRoom.Office.Kill(EntityType);
                    continue;
                }
                else if (office)
                {
                    if (office.ShutterOpen)
                    {
                        office.Kill(EntityType);
                        continue;
                    }
                    else
                    {
                        KickCorkToRoom(7);
                        LampManager.Instance.Flicker();
                        continue;
                    }
                }

                //Movement opportunity
                WalkToNextRoom();
                Log("Went to " + CurrentRoom.name + ".");
            }
        }
    }

    public void KickCorkToRoom(int roomID)
    {
        //Teleport Cork to room
        CurrentRoom.LeaveRoom(EntityType);
        CurrentRoom = RoomController.Instance.GetRoom(roomID);
        CurrentRoom.EnterRoom(EntityType);

        //resume normal behavior
        routeProgress = 0;
        currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, IgnoredRooms);

        Log("Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
    }
}
