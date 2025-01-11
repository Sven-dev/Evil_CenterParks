using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorkAI : Entity
{
    [SerializeField] private Route currentRoute;
    [SerializeField] private int routeProgress = 0;

    private IEnumerator Start()
    {
        print("CorkAI started.");

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetRandomRoom(CurrentRoom);

        while (true)
        {
            int rnd = Random.Range(1, 21);
            if (AILevel > rnd)
            {          
                //Movement opportunity
                routeProgress++;
                CurrentRoom = currentRoute.Path[routeProgress];
                CurrentRoom.EnterRoom(this);

                //Check if you need to recalculate your route
                if (CurrentRoom == currentRoute.Destination)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetRandomRoom(CurrentRoom);
                }
            }
            else
            {
                print("Movement opportunity failed.");
            }

            print("Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
            yield return new WaitForSeconds(MovementOpportunityCooldown);
        }
    }
}
