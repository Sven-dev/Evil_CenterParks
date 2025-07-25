using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abhorwretch : Entity
{
    [Space]
    [SerializeField] private UnityVoidEvent OnKill;

    protected IEnumerator _BehaviourLoop()
    {
        CurrentRoom.EnterRoom(EntityType);
        Log("AI enabled in " + CurrentRoom.name + ".");
        //Always paths Abo to Office
        routeProgress = 0;
        currentRoute = RoomController.Instance.GetCameraRoomPath(CurrentRoom);
        while (true)
        {
            if (CurrentRoom.ID == 1)
            {
                //Checks if Abo is in office and then after a set period of time, if the noise limit is met, kills the player
                yield return new WaitForSecondsRealtime(6.4f - AILevel * 0.1f);
                if (CurrentRoom.GetNoiseLevel() >= 3)
                {
                    OnKill.Invoke();
                    Log("Whoops! You were killed by " + EntityType);
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
                Log("Waiting for next movement opportunity (" + Cooldown + " seconds)");
                yield return new WaitForSecondsRealtime(Cooldown);

                if (MovementOpportunity())
                {
                    WalkToNextRoom();
                    Log("Went to " + CurrentRoom.name + ".");
                }
            }
        }
    }
}