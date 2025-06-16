using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private bool AfterAttackMode = false;
    [SerializeField] private Entity Hallucination;

    private void Start()
    {
        switch(EntityType)
        {
            case EntityType.Cork:
                StartCoroutine(_CorkAI());
                break;
            case EntityType.Vial:
                StartCoroutine(_VialAI());
                break;
            case EntityType.Abhorwretch:
                StartCoroutine(_AbhorwretchAI());
                break;
            case EntityType.Wanderer:
                StartCoroutine(_WandererAI());
                break;
        }
    }

    private void OnEnable()
    {
        switch (EntityType)
        {
            case EntityType.Hallucination:
                StartCoroutine(_HallucinationAI());
                break;
        }
    }

    private void OnDisable()
    {
        CurrentRoom.LeaveRoom(EntityType);
        StopAllCoroutines();
    }

    private IEnumerator _CorkAI()
    {
        CurrentRoom.EnterRoom(EntityType);
        print("<color=Cyan>Cork:</color> AI started in " + CurrentRoom.name + ".");

        List<Room> ignoredRooms = new List<Room>();

        //Hallucination setup
        bool hallucinationMode = false;
        float rnd = UnityEngine.Random.Range(60, 121);     
        TimeSpan hallucinationTimer = DateTime.Now.TimeOfDay + TimeSpan.FromSeconds(UnityEngine.Random.Range(60, 121));
        print("<color=Cyan>Cork:</color> Setting a hallucination timer for " + rnd + " seconds.");

        ignoredRooms.Add(CurrentRoom);
        routeProgress = 0;
        currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, ignoredRooms);
        ignoredRooms.Remove(CurrentRoom); 
        print("<color=Cyan>Cork:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
        
        while (true)
        {
            //Checking ignored rooms
            for (int i = ignoredRooms.Count -1; i >= 0; i--)
            {
                Room room = ignoredRooms[i];
                if (room.GetNoiseLevel() == 0)
                {
                    ignoredRooms.Remove(room);
                }
            }

            //Waiting based on loudest noise level
            List<Room> loudestRooms = RoomController.Instance.GetLoudestRooms(ignoredRooms);
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

            if (hallucinationMode)
            {
                if (CurrentRoom == currentRoute.Destination)
                {
                    //Clear hallucination
                    Hallucination.gameObject.SetActive(false);

                    //Generate new timer
                    rnd = UnityEngine.Random.Range(60, 121);
                    hallucinationTimer = DateTime.Now.TimeOfDay + TimeSpan.FromSeconds(rnd);

                    hallucinationMode = false;
                    print("<color=Cyan>Cork:</color> Hallucination mode done! setting a new hallucination timer for " + rnd + " seconds, and resuming normal behaviour.");
                }
            }
            else if (DateTime.Now.TimeOfDay > hallucinationTimer && CurrentRoom.ID != 1)
            {
                loudestRooms = RoomController.Instance.GetLoudestRooms(ignoredRooms);
                if (CurrentRoom.GetNoiseLevel() == 5 || loudestRooms[0].GetNoiseLevel() < 4)
                {
                    hallucinationMode = true;

                    //Create hallucination
                    Hallucination.gameObject.SetActive(true);

                    //Path to room 1 or 12, whichever is closer
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetRouteTo1or12(CurrentRoom);

                    print("<color=Cyan>Cork:</color> Hallucination mode: Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
            }

            if (!hallucinationMode)
            {
                //if the destination room isn't one of the loudest rooms anymore, recalculate
                loudestRooms = RoomController.Instance.GetLoudestRooms(ignoredRooms);
                if (loudestRooms[0].GetNoiseLevel() != currentRoute.Destination.GetNoiseLevel())
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, ignoredRooms);

                    print("<color=Cyan>Cork:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
                }
            }

            if (MovementOpportunity())
            {
                GuestRoomManager guestRooms = CurrentRoom.GuestRooms;
                OfficeManager office = CurrentRoom.Office;
                if (guestRooms && !guestRooms.Killed)
                {
                    if (guestRooms.BeingDisturbed)
                    {
                        guestRooms.Kill();
                        print("<color=Cyan>Cork:</color> Room " + CurrentRoom + " kill attempt successful.");
                        continue;
                    }
                    else
                    {
                        ignoredRooms.Add(CurrentRoom);
                        print("<color=Cyan>Cork:</color> Room " + CurrentRoom + " kill attempt failed.");
                    }
                }
                else if (CurrentRoom.ID == 12)
                {
                     yield return new WaitForSecondsRealtime(3);
                    {
                           office.Kill();
                           continue;
                       }
                }
                else if (office)
                {
                    if (office.ShutterOpen)
                    {
                        office.Kill();
                        continue;
                    }
                    else
                    {
                        //Teleport Cork to room 7
                        CurrentRoom.LeaveRoom(EntityType);
                        CurrentRoom = RoomController.Instance.GetRoom(7);
                        CurrentRoom.EnterRoom(EntityType);

                        //resume normal behavior
                        routeProgress = 0;
                        currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, ignoredRooms);

                        print("<color=Cyan>Cork:</color> Player kill attempt failed.");
                        print("<color=Cyan>Cork:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);

                        continue;
                    }
                }                
                //Movement opportunity
                WalkToNextRoom();
                print("<color=Cyan>Cork:</color> Went to " + CurrentRoom.name + ".");                         
            }         
        }
    }

    private IEnumerator _VialAI()
    {
        CurrentRoom.EnterRoom(EntityType);
        print("<color=Yellow>Vial:</color> AI started in " + CurrentRoom.name + ".");

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

    private IEnumerator _HallucinationAI()
    {
        CurrentRoom = RoomController.Instance.FindEntity(EntityType.Cork);
        CurrentRoom.EnterRoom(EntityType);
        print("<color=Grey>Hallucination:</color> AI started in " + CurrentRoom.name + ".");
        
        List<Room> ignoredRooms = new List<Room>();
       
        routeProgress = 0;
        currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, ignoredRooms);
        print("<color=Grey>Hallucination:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
        
        while (true)
        {
            //Waiting based on loudest noise level
            List<Room> loudestRooms = RoomController.Instance.GetLoudestRooms(ignoredRooms);
            switch (loudestRooms[0].GetNoiseLevel())
            {
                case 5:
                    print("<color=Grey>Hallucination:</color> Waiting for next movement opportunity (" + 2 + " seconds)");
                    yield return new WaitForSecondsRealtime(2f);
                    break;
                case 4:
                    print("<color=Grey>Hallucination:</color> Waiting for next movement opportunity (" + 4 + " seconds)");
                    yield return new WaitForSecondsRealtime(4f);
                    break;
                default:
                    print("<color=Grey>Hallucination:</color> Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
                    yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);
                    break;
            }

            //if the destination room isn't one of the loudest rooms anymore, recalculate
            loudestRooms = RoomController.Instance.GetLoudestRooms(ignoredRooms);
            if (loudestRooms[0].GetNoiseLevel() != currentRoute.Destination.GetNoiseLevel())
            {               
                routeProgress = 0;
                currentRoute = RoomController.Instance.GetLoudestRoomPath(CurrentRoom, ignoredRooms);
                print("<color=Grey>Hallucination:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);
            }
            
            if (CurrentRoom != currentRoute.Destination)
            {
                if (MovementOpportunity())
                {
                    //Movement opportunity
                    WalkToNextRoom();
                    print("<color=Grey>Hallucination:</color> Went to " + CurrentRoom.name + ".");
                }
            }
        }
    }

    private IEnumerator _WandererAI()
    {
        CurrentRoom.EnterRoom(EntityType);
        print("<color=Green>Wanderer:</color> AI started in " + CurrentRoom.name + ".");

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
    private IEnumerator _AbhorwretchAI()
    {
        CurrentRoom.EnterRoom(EntityType);
        print("<color=Green>Abo:</color> AI started in " + CurrentRoom.name + ".");

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetCameraRoomPath(CurrentRoom);
        while (true)
        {
            print("<color=Green>Abo:</color> Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");

           OfficeManager office = CurrentRoom.Office;
           if (office)
           {
               yield return new WaitForSecondsRealtime(3 - AILevel * 0.1f);
               if (CurrentRoom.GetNoiseLevel() > 2)
               {
                   office.Kill();
                   continue;
               }
               else
               {
                   //Teleport Cork to room 7
                   CurrentRoom.LeaveRoom(EntityType);
                   CurrentRoom = RoomController.Instance.GetRoom(7);
                   CurrentRoom.EnterRoom(EntityType);

                   //resume normal behavior
                   routeProgress = 0;
                   currentRoute = RoomController.Instance.GetCameraRoomPath(CurrentRoom);

                   print("<color=Green>Abo:</color> Player kill attempt failed.");
                   print("<color=Green>Abo:</color> Calculating route from " + currentRoute.Start.name + " to " + currentRoute.Destination.name);

                   continue;
               }     
          }
          else 
          {
            yield return new WaitForSecondsRealtime(MovementOpportunityCooldown);

            if (MovementOpportunity()) 

                WalkToNextRoom();
                print("<color=Green>Abo:</color> Went to " + CurrentRoom.name + ".");
          }   
        }
    }

    private bool MovementOpportunity(int max = 21)
    {
        int rnd = UnityEngine.Random.Range(1, max);
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
                case EntityType.Abhorwretch:
                    color = "Green";
                    break;
                case EntityType.Wanderer:
                    color = "Green";
                    break;
                case EntityType.Hallucination:
                    color = "Grey";
                    break;
            }

            print("<color=" + color + ">" + EntityType.ToString() + ":</color> Movement opportunity failed.");

            return false;
        }
    }

    private void WalkToNextRoom()
    {
        //If you're already at the destination, don't move.
        if (currentRoute.Distance != 0 && routeProgress != currentRoute.Distance)
        {
            CurrentRoom.LeaveRoom(EntityType);
            routeProgress++;
            CurrentRoom = currentRoute.Path[routeProgress];
            CurrentRoom.EnterRoom(EntityType);
        }
    }

    public void CheckForAILevelIncrease(TimeSpan currentTime)
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

            case EntityType.Hallucination:
                if (currentTime.Hours == 2 || currentTime.Hours == 3 || currentTime.Hours == 4)
                {
                    AILevel++;
                }
                break;
        }
    }

    public void KickOutOfOffice()
    {
            CurrentRoom.LeaveRoom(EntityType);
            CurrentRoom = RoomController.Instance.GetRoom()
            CurrentRoom.EnterRoom(EntityType);
            routeProgress = 0;
            currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom);
            print("<color=Yellow>Vial:</color> Got kicked out of " + currentRoute.Start.name + ", to " + currentRoute.Destination.name);
    }
}

/*   Previous Iteration
    public void KickOutOfOffice()
    {
        if (CurrentRoom.ID == 1)
        {
            routeProgress = 0;
            currentRoute = RoomController.Instance.GetFurthestQuietestRoomPath(CurrentRoom);
            WalkToNextRoom();
            print("<color=Yellow>Vial:</color> Got kicked out of " + currentRoute.Start.name + ", to " + currentRoute.Destination.name);
        }
    }
}
*/
public enum EntityType
{
    Cork,
    Vial,
    Abhorwretch,
    Wanderer,
    Hallucination
}
