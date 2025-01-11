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
        }
    }

    private IEnumerator CorkAI()
    {
        print("CorkAI started.");
        CurrentRoom.EnterRoom(EntityType);

        routeProgress = 0;
        currentRoute = RoomController.Instance.GetRandomRoom(CurrentRoom);
        while (true)
        {
            int rnd = Random.Range(1, 21);
            if (AILevel > rnd)
            {
                //Movement opportunity
                routeProgress++;
                CurrentRoom.LeaveRoom(EntityType);
                CurrentRoom = currentRoute.Path[routeProgress];
                CurrentRoom.EnterRoom(EntityType);

                //Check if you need to recalculate your route
                if (CurrentRoom == currentRoute.Destination)
                {
                    routeProgress = 0;
                    currentRoute = RoomController.Instance.GetRandomRoom(CurrentRoom);
                }
            }
            else
            {
                print("Movement opportunity failed.");
            }

            print("Waiting for next movement opportunity (" + MovementOpportunityCooldown + " seconds)");
            yield return new WaitForSeconds(MovementOpportunityCooldown);
        }
    }
}

public enum EntityType
{
    Cork
}