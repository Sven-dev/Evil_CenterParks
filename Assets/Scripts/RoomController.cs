using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private List<Room> Rooms;

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
}