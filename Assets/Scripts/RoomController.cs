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

    public List<Room> GetLoudestRooms(List<Room> ignoredRooms)
    {
        int loudestNoiseLevel = -1;
        List<Room> loudestRooms = new List<Room>();

        foreach (Room room in Rooms)
        {
            if (ignoredRooms.Contains(room))
            {
                continue;
            }

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

    public Route GetLoudestRoomPath(Room currentRoom, List<Room> ignoredRooms)
    {
        //Get the loudest rooms
        List<Room> LoudestRooms = GetLoudestRooms(ignoredRooms);

        //Get the shortest route to each room
        List<Route> routes = new List<Route>();
        foreach (Room loudRoom in LoudestRooms)
        {
            List<Route> possibleRoutes = FindShortestRoutes(currentRoom, loudRoom);
            routes.Add(possibleRoutes[Random.Range(0, possibleRoutes.Count)]);
        }

        //Return a random shortest route to one of the loudest rooms
        return routes[Random.Range(0, routes.Count)];
    }

    public Route GetFurthestQuietestRoomPath(Room currentRoom, List<Room> ignoredRooms)
    {
        //Filter through all rooms until you have the ones with the lowest noise level
        int loudestNoiseLevel = 6;
        List<Room> quietestRooms = new List<Room>();
        quietestRooms.Add(GetRoom(1));
        foreach (Room room in Rooms)
        {
            if (room.GuestRooms == null || room == currentRoom || ignoredRooms.Contains(room))
            {
                continue;
            }

            //Make sure the character doesn't try to path to the room they're in
            if (room == currentRoom)
            {
                continue;
            }

            int roomNoise = room.GetNoiseLevel();
            if (roomNoise < loudestNoiseLevel)
            {
                loudestNoiseLevel = roomNoise;
                quietestRooms.Clear();

                quietestRooms.Add(room);
            }
            else if (roomNoise == loudestNoiseLevel)
            {
                quietestRooms.Add(room);
            }
        }

        //Calculate all possible routes
        List<Route> routes = new List<Route>();
        foreach (Room targetRoom in quietestRooms)
        {
            List<Route> possibleRoutes = FindShortestRoutes(currentRoom, targetRoom);
            //Per room, pick a random route (avoids bias towards rooms with multiple entrances)
            routes.Add(possibleRoutes[Random.Range(0, possibleRoutes.Count)]);
        }

        //Out of the shortest route to each room, find and pick the longest ones
        List<Route> furthestRoutes = new List<Route>();
        foreach (Route route in routes)
        {
            if (furthestRoutes.Count == 0 || route.Distance > furthestRoutes[0].Distance)
            {
                furthestRoutes.Clear();
                furthestRoutes.Add(route);
            }
            else if (route.Distance == furthestRoutes[0].Distance)
            {
                furthestRoutes.Add(route);
            }
        }

        //return a random route
        return furthestRoutes[Random.Range(0, furthestRoutes.Count)];
    }

    public Route GetRandomRoomPath(Room currentRoom)
    {
        Room targetRoom = currentRoom;
        while (targetRoom == currentRoom)
        {
            targetRoom = Rooms[Random.Range(0, Rooms.Count)];
        }

        List<Route> routes = FindShortestRoutes(currentRoom, targetRoom);
        return routes[Random.Range(0, routes.Count)];
    }

    public Route GetRandomGuestRoomPath(Room currentRoom)
    {
        Room targetRoom = currentRoom;
        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                targetRoom = GetRoom(5);
                break;
            case 1:
                targetRoom = GetRoom(6);
                break;
            case 2:
                targetRoom = GetRoom(10);
                break;
        }

        while (targetRoom == currentRoom)
        {
            targetRoom = Rooms[Random.Range(0, Rooms.Count)];
        }

        List<Route> routes = FindShortestRoutes(currentRoom, targetRoom);
        return routes[Random.Range(0, routes.Count)];
    }

    public Route GetCameraRoomPath(Room currentRoom)
    {
        //Get the cameraRoom
        Room targetRoom = Rooms[0];
        if (currentRoom == targetRoom)
        {
            throw new System.Exception("Pathfinding error: target room cannot be the same as current room");
        }

        //Find the shortest route
        List<Route> routes = FindShortestRoutes(currentRoom, targetRoom);

        //If there's multiple possible routes, prioritize the one that goes through room 2 (index 1 in the Rooms list)
        if (routes.Count > 1)
        {
            foreach (Route route in routes)
            {
                if (route.Path.Contains(Rooms[1]))
                {
                    return route;
                }
            }
        }

        return routes[0];
    }

    public Route GetRouteTo1or12(Room currentRoom)
    {
        List<Route> routesTo1 = FindShortestRoutes(currentRoom, Rooms[0]);
        List<Route> routesTo12 = FindShortestRoutes(currentRoom, Rooms[11]);

        if (routesTo1[0].Distance < routesTo12[0].Distance)
        {
            return routesTo1[Random.Range(0, routesTo1.Count)];
        }

        return routesTo12[Random.Range(0, routesTo12.Count)];
    }

    public Route GetRouteTo7(Room currentRoom)
    {
        List<Route> routesTo7 = FindShortestRoutes(currentRoom, Rooms[6]);
        return routesTo7[Random.Range(0, routesTo7.Count)];
    }

    public Route GetRouteTo2(Room currentRoom)
    {
        List<Route> routesTo2 = FindShortestRoutes(currentRoom, Rooms[1]);
        return routesTo2[Random.Range(0, routesTo2.Count)];
    }

    private List<Route> FindShortestRoutes(Room start, Room end)
    {
        //Get all possible routes
        List<Route> possibleRoutes = CalculateRoutes(new Route(start, end));

        //Find the shortest one
        List<Route> shortestRoutes = new List<Route>();
        foreach (Route route in possibleRoutes)
        {
            //only check routes that actually reach the destination
            if (route.ReachesDestination)
            {
                //if there isn't a shortest route yet, this becomes the default one
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

    private List<Route> FindLongestRoutes(Room start, Room end)
    {
        //Get all possible routes
        List<Route> possibleRoutes = CalculateRoutes(new Route(start, end));

        //Find the longest one
        List<Route> longestRoutes = new List<Route>();
        foreach (Route route in possibleRoutes)
        {
            if (route.ReachesDestination)
            {
                //If there isn't a longest route yet, this one becomes the default one
                if (longestRoutes.Count == 0)
                {
                    longestRoutes.Add(route);
                }
                //If the distance is the same as the longest one, add it to the list
                else if (route.Distance == longestRoutes[0].Distance)
                {
                    longestRoutes.Add(route);
                }
                //If the distance is longer, empty the list and add this one instead
                else if (route.Distance > longestRoutes[0].Distance)
                {
                    longestRoutes.Clear();
                    longestRoutes.Add(route);
                }
            }
        }

        return longestRoutes;
    }

    public Room GetRoom(int ID)
    {
        return Rooms[ID];
    }

    public Room FindEntity(EntityType entity)
    {
        foreach(Room room in Rooms)
        {
            if (room.Entities.Contains(entity))
            {
                return room;
            }
        }

        //If an entity can't be found (which should never happen), default to camera 8.
        return Rooms[9];
    }

    private List<Route> CalculateRoutes(Route route)
    {
        //Check every connected room
        List<Route> routes = new List<Route>();

        //If the start of the room is the same as the destination, return a path with a length of 0 to prevent errors.
        if (route.Start == route.Destination)
        {
            route.ReachesDestination = true;
            routes.Add(route);
            return routes;
        }

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
        Distance = 0;
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
