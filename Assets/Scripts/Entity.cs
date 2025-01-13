using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityType EntityType;
    [Range(1, 20)]
    [SerializeField] protected int AILevel = 10;
    [Range(1, 10)]
    [SerializeField] protected float MovementOpportunityCooldown = 4.9f;
    //[SerializeField] public bool NoiseMaker = false;    
    [Space]
    [SerializeField] private Room CurrentRoom;
    private Route currentRoute;
    private int routeProgress = 0;

    private void Start()
    {
        switch(EntityType)
        {
            case EntityType.Cork:
                StartCoroutine(CorkAI());
                break;
            case EntityType.Vial:
                StartCoroutine(VialAI());
                break;
        }
    }

    private IEnumerator CorkAI()
    {
        print("<color=cyan>Cork:</color> AI started in " + CurrentRoom.name + ".");
        CurrentRoom.EnterRoom(EntityType);

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetRandomRoomPath(CurrentRoom);
        while (true)
        {
            print("<color=cyan>Cork:</color> Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
            yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);

            if (MovementOpportunity())
            {
                //Movement opportunity
                routeProgress++;
                CurrentRoom.LeaveRoom(EntityType);
                CurrentRoom = currentRoute.Path[routeProgress];
                CurrentRoom.EnterRoom(EntityType);
                print("<color=Cyan>Cork:</color> Went to " + CurrentRoom.name + ".");

                //Check if you need to recalculate your route
                if (CurrentRoom == currentRoute.Destination)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetRandomRoomPath(CurrentRoom);
                    print("<color=Cyan>Cork:</color> Calculating route from" + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
            }
        }
    }

    private IEnumerator VialAI()
    {
        print("<color=Yellow>Vial:</color> AI started in " + CurrentRoom.name + ".");
        CurrentRoom.EnterRoom(EntityType);

        routeProgress = 0;
        while (true)
        {
            if (CurrentRoom.ID == 1)
            {
                print("<color=Yellow>Vial:</color> Vial is in the camera room, and will not move until removed");
                yield return new WaitForSecondsRealtime(1f);
                continue;
            }

            for (int i = 1; i < 3; i++)
            {
                if (MovementOpportunity(21 * i))
                {
                    if (currentRoute != null && CurrentRoom != currentRoute.Destination)
                    {
                        routeProgress++;
                        CurrentRoom.LeaveRoom(EntityType);
                        CurrentRoom = currentRoute.Path[routeProgress];
                        CurrentRoom.EnterRoom(EntityType);
                        print("<color=Yellow>Vial:</color> Went to " + CurrentRoom.name + ".");
                    }
                    else if (CurrentRoom.GetNoiseLevel() < 5)
                    {
                        CurrentRoom.AlterNoiseLevel(+1);
                        print("<color=Yellow>Vial:</color> Raising " + CurrentRoom.name + " noise level to " + CurrentRoom.GetNoiseLevel() + ".");
                    }
                    else
                    {
                        int rnd = Random.Range(1, 5);
                        if (rnd == 1)
                        {
                            routeProgress = 0;
                            currentRoute = RoomController.Instance.GetCameraRoomPath(CurrentRoom);
                            print("<color=Yellow>Vial:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                        }
                        else if (rnd >= 2)
                        {
                            routeProgress = 0;
                            currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom);
                            print("<color=Yellow>Vial:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                        }

                        routeProgress++;
                        CurrentRoom.LeaveRoom(EntityType);
                        CurrentRoom = currentRoute.Path[routeProgress];
                        CurrentRoom.EnterRoom(EntityType);
                        print("<color=Yellow>Vial:</color> Went to " + CurrentRoom.name + ".");
                    }
                }

                yield return new WaitForSecondsRealtime(0.1f);
            }
            
            print("<color=Yellow>Vial:</color> Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
            yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);
        }
    }

    private bool MovementOpportunity(int max = 21)
    {
        int rnd = Random.Range(1, max);
        if (AILevel > rnd)
        {
            return true;
        }
        else
        {
            string color = "";
            switch(EntityType)
            {
                case EntityType.Cork:
                    color = "Cyan";
                    break;
                case EntityType.Vial:
                    color = "yellow";
                    break;
            }

            print("<color=" + color + ">" + EntityType.ToString() + ":</color> Movement opportunity failed.");

            return false;
        }
    }  
}

public enum EntityType
{
    Cork,
    Vial
}