using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abhorwretch : Entity
{
    protected IEnumerator _BehaviourLoop()
    {
        CurrentRoom.EnterRoom(EntityType);
        Log("AI enabled in " + CurrentRoom.name + ".");

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetCameraRoomPath(CurrentRoom);
        while (true)
        {
            OfficeManager office = CurrentRoom.Office;
            if (office)
            {
                yield return new WaitForSecondsRealtime(3 - AILevel * 0.1f);
                if (CurrentRoom.GetNoiseLevel() > 2)
                {
                    office.Kill(EntityType);
                    continue;
                }
                else
                {
                    //Teleport Abo to room 7
                    CurrentRoom.LeaveRoom(EntityType);
                    CurrentRoom = RoomController.Instance.GetRoom(7);
                    CurrentRoom.EnterRoom(EntityType);

                    //resume normal behaviour
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetCameraRoomPath(CurrentRoom);

                    Log("Player kill attempt failed.");
                    Log("Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);

                    continue;
                }
            }
            else
            {
                Log("Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
                yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);

                if (MovementOpportunity())
                {
                    WalkToNextRoom();
                    Log("Went to " + CurrentRoom.name + ".");
                }
            }
        }
    }
}