using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GuestSystem
{
    public class GuestManager : MonoBehaviour
    {
        public static GuestManager Instance;

        [SerializeField] private int RandomGuests = 10;
        [SerializeField] private List<ArrivalData> Guests;
        [SerializeField] private NameGenerator NameGenerator;
        [SerializeField] private Transform GuestListWrapper;
        [Space]
        [SerializeField] private Transform DoorPivot;
        [SerializeField] private Transform ShutterPivot;
        [SerializeField] private Transform ParkPivot;
        [Space]
        [SerializeField] private GuestListItem GuestListPrefab;
        [Space]
        [SerializeField] private UnityVoidEvent OnGuestAtDoor;

        [SerializeField] private Entity GuestEntity;
        [SerializeField] private Entity WandererEntity;

        private Guest ActiveGuest;

        private void Start()
        {
            //Create a guest database
            List<GuestData> database = new List<GuestData>();

            //Add the preprogrammed guests to the guest list database
            foreach (ArrivalData guest in Guests)
            {
                if (guest.ValidGuest)
                {
                    database.Add(guest.Info);
                }
            }

            //Generate a bunch of extra guests
            for (int i = 0; i < RandomGuests; i++)
            {
                string name = NameGenerator.GenerateName();
                int refID = i;
                int room = UnityEngine.Random.Range(1, 31);

                Date arrival = Date.Random();
                Date departure = Date.Random(arrival);

                //GuestArrivalData
                database.Add(new GuestData(name, refID, room, arrival, departure));
            }

            //Shuffle the database
            System.Random rnd = new System.Random();
            database = database.OrderBy(item => rnd.Next()).ToList();

            int rndRef = UnityEngine.Random.Range(10000, 20000);
            for (int i = 0; i <  database.Count; i++)
            {
                GuestData guest = database[i];
                guest.Reference = rndRef + i;
            }

            //Sort the guest list database by Room
            database = database.OrderBy(info => info.Room).ToList();

            //Create the pc display
            foreach (GuestData info in database)
            {
                GuestListItem item = Instantiate(GuestListPrefab, GuestListWrapper);
                item.SetData(info);
            }
        }

        public void CheckForGuestArrival(TimeSpan currentTime)
        {
            foreach (ArrivalData guest in Guests)
            {
                TimeSpan arrival = new TimeSpan(1, guest.ArrivalHour, guest.ArrivalMinute, 0);
                if (!guest.Arrived && currentTime >= arrival)
                {
                    ActiveGuest = Instantiate(guest.Prefab, DoorPivot.position, Quaternion.Euler(0, 90, 0), transform);
                    ActiveGuest.SetData(guest);

                    guest.Arrived = true;
                    OnGuestAtDoor?.Invoke();
                }
            }
        }

        public void MoveGuestToShutter()
        {
            ActiveGuest.MoveTo(ShutterPivot);
        }

        public void MoveGuestToDoor()
        {
            ActiveGuest.MoveTo(DoorPivot);
            Invoke("UnloadGuest", 1f);
        }

        public void MoveGuestToPark()
        {
            ActiveGuest.MoveTo(ParkPivot);
            Invoke("UnloadGuest", 1f);
      
            if (ActiveGuest.Data.ValidGuest)
            {
                GuestEntity.gameObject.SetActive(true);
            }
            else
            {
                WandererEntity.gameObject.SetActive(true);
            }
        }

        private void UnloadGuest()
        {
            Destroy(ActiveGuest.gameObject);
        }
    }
}