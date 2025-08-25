using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : Entity
{
    protected IEnumerator _BehaviourLoop()
    {
        CurrentRoom = RoomController.Instance.GetRoom(2);
        CurrentRoom.EnterRoom(EntityType);
        Log("AI enabled in " + CurrentRoom.name + ".");

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetRandomGuestRoomPath(CurrentRoom);
        while (true)
        {
            yield return new WaitForSecondsRealtime(5f);
            if (CurrentRoom == currentRoute.Destination)
            {
                Log("Arrived at destination (" + currentRoute.Destination.name + ")");
                gameObject.SetActive(false);                   
            }
            else
            {
                WalkToNextRoom();
                Log("Went to " + CurrentRoom.name + ".");
            }
        }
    }
}