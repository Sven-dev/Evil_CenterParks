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

    public Route GetLoudestRoomPath(Room currentRoom)
    {
        //Get the loudest rooms
        int loudestNoiseLevel = -1;
        List<Room> LoudestRooms = new List<Room>();
        foreach (Room room in Rooms)
        {
            int noiseLevel = room.GetNoiseLevel();
            if (noiseLevel > loudestNoiseLevel)
            {
                loudestNoiseLevel = noiseLevel;
                LoudestRooms.Clear();
                LoudestRooms.Add(room);
            }
            else if (noiseLevel == loudestNoiseLevel)
            {
                LoudestRooms.Add(room);
            }
        }

        //Get the shortest route to each room
        List<Route> routes = new List<Route>();
        foreach (Room loudRoom in LoudestRooms)
        {
            //Filter out the room you're currently in, if that's one of the loudest rooms
            if (loudRoom == currentRoom)
            {
                continue;
            }

            List<Route> possibleRoutes = FindShortestRoutes(currentRoom, loudRoom);
            routes.Add(possibleRoutes[Random.Range(0, possibleRoutes.Count)]);
        }

        //Return a random shortest route to one of the loudest rooms
        return routes[Random.Range(0, routes.Count)];
    }

    public Route GetFurthestQuietestRoomPath(Room currentRoom)
    {
        //Filter through all rooms until you have the ones with the lowest noise level
        int loudestNoiseLevel = 6;
        List<Room> quietestRooms = new List<Room>();
        foreach(Room room in Rooms)
        {
            if (!room.NoiseRoomCheck())
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
        foreach(Room targetRoom in quietestRooms)
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