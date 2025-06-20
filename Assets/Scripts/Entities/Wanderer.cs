using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Entity
{
    protected IEnumerator _BehaviourLoop()
    {
        CurrentRoom.EnterRoom(EntityType);
        Log("AI enabled in " + CurrentRoom.name + ".");

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetRandomRoomPath(CurrentRoom);
        while (true)
        {
            print("<color=Green>Wanderer:</color> Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
            yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);

            if (MovementOpportunity())
            {
                //Movement opportunity
                WalkToNextRoom();
                print("<color=Green>Wanderer:</color> Went to " + CurrentRoom.name + ".");

                //Check if you need to recalculate your route
                if (CurrentRoom == currentRoute.Destination)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetRandomRoomPath(CurrentRoom);
                    print("<color=Green>Wanderer:</color> Calculating route from" + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
            }
        }
    }
}