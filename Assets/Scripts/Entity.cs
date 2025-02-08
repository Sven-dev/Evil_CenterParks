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
            case EntityType.Wanderer:
                StartCoroutine(WandererAI());
                break;
        }
    }

    private IEnumerator CorkAI()
    {
        print("<color=Cyan>Cork:</color> AI started in " + CurrentRoom.name + ".");
        CurrentRoom.EnterRoom(EntityType);

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom);
        print("<color=Cyan>Cork:</color> Calculating route from" + currentRoute.Start.name + " to " + currentRoute.Destination.name);
        while (true)
        {
            if (CurrentRoom.ID == 1)
            {
                print("<color=Cyan>Cork:</color> Cork is in the camera room, and will not move until he has done a kill attempt");
                yield return new WaitForSecondsRealtime(1f);
                continue;
            }

            List<Room> loudestRooms = RoomController.Instance.GetLoudestRooms();
            switch (loudestRooms[0].GetNoiseLevel())
            {
                case 5:
                    print("<color=Cyan>Cork:</color> Waiting for next movement opportunity (" + 2 + " seconds)");
                    yield return new WaitForSecondsRealtime(2f);
                    break;
                case 4:
                    print("<color=Cyan>Cork:</color> Waiting for next movement opportunity (" + 4 + " seconds)");
                    yield return new WaitForSecondsRealtime(4f);
                    break;
                default:
                    print("<color=Cyan>Cork:</color> Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
                    yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);
                    break;
            }

            //if the destination room isn't one of the loudest rooms anymore, recalculate
            if (loudestRooms[0].GetNoiseLevel() != currentRoute.Destination.GetNoiseLevel())
            {
                routeProgress = 0;
                currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom);
                print("<color=Cyan>Cork:</color> Calculating route from" + currentRoute.Start.name + " to " + currentRoute.Destination.name);
            }

            if (CurrentRoom != currentRoute.Destination)
            {
                if (MovementOpportunity())
                {
                    //Movement opportunity
                    routeProgress++;
                    CurrentRoom.LeaveRoom(EntityType);
                    CurrentRoom = currentRoute.Path[routeProgress];
                    CurrentRoom.EnterRoom(EntityType);
                    print("<color=Cyan>Cork:</color> Went to " + CurrentRoom.name + ".");
                }
            }
        }
    }

    private IEnumerator VialAI()
    {
        print("<color=Yellow>Vial:</color> AI started in " + CurrentRoom.name + ".");
        CurrentRoom.EnterRoom(EntityType);

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom);
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
                int rnd = Random.Range(1, 5);
                if (rnd == 1)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetCameraRoomPath(CurrentRoom);
                    print("<color=Yellow>Vial:</color> Beelining it from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
                else if (rnd >= 2)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom);
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
                        routeProgress++;
                        CurrentRoom.LeaveRoom(EntityType);
                        CurrentRoom = currentRoute.Path[routeProgress];
                        CurrentRoom.EnterRoom(EntityType);
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

    private IEnumerator WandererAI()
    {
        print("<color=Green>Wanderer:</color> AI started in " + CurrentRoom.name + ".");
        CurrentRoom.EnterRoom(EntityType);

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetRandomRoomPath(CurrentRoom);
        while (true)
        {
            print("<color=Green>Wanderer:</color> Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
            yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);

            if (MovementOpportunity())
            {
                //Movement opportunity
                routeProgress++;
                CurrentRoom.LeaveRoom(EntityType);
                CurrentRoom = currentRoute.Path[routeProgress];
                CurrentRoom.EnterRoom(EntityType);
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
                    color = "Yellow";
                    break;
                case EntityType.Wanderer:
                    color = "Green";
                    break;
            }

            print("<color=" + color + ">" + EntityType.ToString() + ":</color> Movement opportunity failed.");

            return false;
        }
    }  

    public void CheckForAILevelIncrease(System.TimeSpan currentTime)
    {
        switch (EntityType)
        {
            case EntityType.Cork:
                if (currentTime.Hours == 2 || currentTime.Hours == 3 || currentTime.Hours == 4)
                {
                    AILevel++;
                }
                break;

            case EntityType.Vial:
                if (currentTime.Hours == 2 || currentTime.Hours == 4)
                {
                    AILevel++;
                }
                break;
        }
    }
}

public enum EntityType
{
    Cork,
    Vial,
    Wanderer
}