using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallucination : Entity
{
    private List<Room> IgnoredRooms = new List<Room>();

    protected IEnumerator _BehaviourLoop()
    {
        CurrentRoom = RoomController.Instance.FindEntity(EntityType.Cork);
        CurrentRoom.EnterRoom(EntityType);
        Log("AI enabled in " + CurrentRoom.name + ".");

        IgnoredRooms = new List<Room>();

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, IgnoredRooms);
        Log("Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);

        while (true)
        {
            //Waiting based on loudest noise level
            List<Room> loudestRooms = RoomController.Instance.GetLoudestRooms(IgnoredRooms);
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

            //if the destination room isn't one of the loudest rooms anymore, recalculate
            loudestRooms = RoomController.Instance.GetLoudestRooms(IgnoredRooms);
            if (loudestRooms[0].GetNoiseLevel() != currentRoute.Destination.GetNoiseLevel())
            {
                routeProgress = 0;
                currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, IgnoredRooms);
                Log("Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
            }

            if (CurrentRoom != currentRoute.Destination)
            {
                if (MovementOpportunity())
                {
                    //Movement opportunity
                    WalkToNextRoom();
                    Log("Went to " + CurrentRoom.name + ".");
                }
            }
        }
    }
}
