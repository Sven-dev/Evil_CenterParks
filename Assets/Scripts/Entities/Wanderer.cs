using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Entity
{
    protected IEnumerator _BehaviourLoop()
    {
        CurrentRoom = RoomController.Instance.GetRoom(2);
        CurrentRoom.EnterRoom(EntityType);
        Log("AI enabled in " + CurrentRoom.name + ".");

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetRandomRoomPath(CurrentRoom);
        while (true)
        {
            Log("Waiting for next movement opportunity (" + 5 + " seconds)");
            yield return new WaitForSecondsRealtime(5);

            if (MovementOpportunity())
            {
                //Movement opportunity
                WalkToNextRoom();
                Log("Went to " + CurrentRoom.name + ".");

                //If in the same room as Cork/Abo, die and increase bloodlust
                if (RoomController.Instance.FindEntity(EntityType.Cork) == CurrentRoom || RoomController.Instance.FindEntity(EntityType.Abhorwretch) == CurrentRoom)
                {
                    Entities.Instance.IncreaseBloodLust();
                    //To do: Scream
                    break;
                }

                //Check if you need to recalculate your route
                if (CurrentRoom == currentRoute.Destination)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetRandomRoomPath(CurrentRoom);
                    Log("Calculating route from" + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
            }
        }

        yield return new WaitForSecondsRealtime(3f);
        gameObject.SetActive(false);
    }
}