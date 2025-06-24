using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vial : Entity
{
    private List<Room> IgnoredRooms = new List<Room>();

    protected IEnumerator _BehaviourLoop()
    {
        yield return null;

        CurrentRoom.EnterRoom(EntityType);
        Log("AI enabled in " + CurrentRoom.name + ".");

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom, IgnoredRooms);
        print("<color=Yellow>Vial:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
        while (true)
        {
            if (CurrentRoom.ID == 1)
            {
                print("<color=Yellow>Vial:</color> Vial is in the camera room, and will not move until removed");
                yield return new WaitForSecondsRealtime(1f);
                continue;
            }

            if (CurrentRoom == currentRoute.Destination && CurrentRoom.GetNoiseLevel() == 5)
            {
                //Recalculate path
                int rnd = UnityEngine.Random.Range(1, 5);
                if (rnd == 1)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetCameraRoomPath(CurrentRoom);
                    print("<color=Yellow>Vial:</color> Beelining it from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
                else if (rnd >= 2)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom, IgnoredRooms);
                    print("<color=Yellow>Vial:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
            }

            print("<color=Yellow>Vial:</color> Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
            yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);

            //Movement opportunites (Vial takes 2)
            for (int i = 1; i < 3; i++)
            {
                if (MovementOpportunity(21 * i))
                {
                    if (CurrentRoom != currentRoute.Destination)
                    {
                        //Walking
                        WalkToNextRoom();
                        print("<color=Yellow>Vial:</color> Went to " + CurrentRoom.name + ".");
                    }
                    else if (CurrentRoom.GetNoiseLevel() < 5)
                    {
                        //Noise raising
                        CurrentRoom.AlterNoiseLevel(+1);
                        print("<color=Yellow>Vial:</color> Raising " + CurrentRoom.name + " noise level to " + CurrentRoom.GetNoiseLevel() + ".");
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
        Log("Got kicked out of " + currentRoute.Start.name + ", to " + currentRoute.Destination.name);
    }

    public void AddIgnoredRoom(Room room)
    {
        IgnoredRooms.Add(room);
    }
}
