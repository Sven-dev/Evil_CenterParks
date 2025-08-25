using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuestSystem
{
    [System.Serializable]
    public class GuestData
    {
        public string Name;
        public int Reference;
        public int Room;
        public Date Arrival;
        public Date Departure;

        public GuestData(string name, int reference, int room, Date arrival, Date departure)
        {
            Name = name;
            Reference = reference;
            Room = room;
            Arrival = arrival;
            Departure = departure;
        }
    }
}