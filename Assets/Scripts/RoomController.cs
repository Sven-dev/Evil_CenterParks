using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController Instance;

    [SerializeField] private List<Room> Rooms;

    private void Awake()
    {
        Instance = this;
    }

    public List<Room> GetLoudestRooms()
    {
        int loudestNoiseLevel = -1;
        List<Room> loudestRooms = new List<Room>();

        foreach (Room room in Rooms)
        {
            int roomNoiseLevel = room.GetNoiseLevel();
            if (roomNoiseLevel == loudestNoiseLevel)
            {
                loudestRooms.Add(room);
            }
            else if (roomNoiseLevel > loudestNoiseLevel)
            {
                loudestRooms.Clear();
                loudestRooms.Add(room);

                loudestNoiseLevel = roomNoiseLevel;
            }
        }

        return loudestRooms;
    }

    public Route GetRandomRoom(Room current)
    {
        Room targetRoom = current;
        while (targetRoom == current)
        {
            targetRoom = Rooms[Random.Range(0, Rooms.Count)];
        }

        print("Current room: " + current.gameObject.name + ", target room: " + targetRoom.gameObject.name);
        print("Calculating routes...");

        List<Route> routes = FindShortestRoutes(current, targetRoom);

        return routes[Random.Range(0, routes.Count)];
    }

    private List<Route> FindShortestRoutes(Room current, Room target)
    {
        //Get all possible routes
        List<Route> possibleRoutes = CalculateRoutes(new Route(current, target));

        //Find the shortest one
        List<Route> shortestRoutes = new List<Route>();
        foreach (Route route in possibleRoutes)
        {
            //only check routes that actually reach the destination
            if (route.ReachesDestination)
            {
                //if there isn't a shortest round yet, this becomes the default one
                if (shortestRoutes.Count == 0)
                {
                    shortestRoutes.Add(route);
                }
                //If the distance is the same as the shortest one, add it to the list
                else if (route.Distance == shortestRoutes[0].Distance)
                {
                    shortestRoutes.Add(route);
                }
                //If the distance is shorter, empty the list and add this one instead
                else if (route.Distance < shortestRoutes[0].Distance)
                {
                    shortestRoutes.Clear();
                    shortestRoutes.Add(route);
                }
            }
        }

        return shortestRoutes;
    }

    private List<Route> CalculateRoutes(Route route)
    {
        //Check every connected room
        List<Route> routes = new List<Route>();
        foreach (Room connectedRoom in route.Path[route.Path.Count - 1].ConnectedRooms)
        {
            //If the connected room is already in the path (meaning it's a dead end or a loop), end the route
            if (route.Path.Contains(connectedRoom))
            {
                continue;
            }

            //Create a copy of the route and add the room as part of the path
            Route routeCopy = new Route(route);
            routeCopy.Path.Add(connectedRoom);
            routeCopy.Distance += 1;

            if (connectedRoom == route.Destination)
            {
                //If it connects to the destination,
                routeCopy.ReachesDestination = true;
                routes.Add(routeCopy);
            }
            else
            {
                //If it doesn't, keep searching.
                routes.AddRange(CalculateRoutes(routeCopy));
                //(this adds all possible routes that return to a list, it's a little complicated to explain lol)
            }
        }

        //Filter out routes that don't reach the destination
        List<Route> returnRoutes = new List<Route>();
        foreach(Route r in routes)
        {
            if (r.ReachesDestination)
            {
                returnRoutes.Add(r);
            }
        }

        //Return all possible routes
        return routes;
    }
}

[System.Serializable]
public class Route
{
    public Room Start;
    public Room Destination;
    public List<Room> Path;

    public int Distance;
    public bool ReachesDestination;

    public Route(Room start, Room destination)
    {
        Start = start;
        Destination = destination;
        Path = new List<Room>();
        Path.Add(start);

        ReachesDestination = false;
        Distance = 1;
    }

    public Route(Route original)
    {
        Start = original.Start;
        Destination = original.Destination;
        Path = new List<Room>();
        Path.AddRange(original.Path);

        ReachesDestination = original.ReachesDestination;
        Distance = original.Distance;
    }
}