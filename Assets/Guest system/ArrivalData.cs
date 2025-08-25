using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuestSystem
{
    [System.Serializable]
    public class ArrivalData
    {
        public Guest Prefab;
        [Space]
        public bool ValidGuest = true;
        [Range(0, 6)] public int ArrivalHour;
        [Range(0, 59)] public int ArrivalMinute;
        public GuestData Info;

        [HideInInspector] public bool Arrived = false;
    }
}
