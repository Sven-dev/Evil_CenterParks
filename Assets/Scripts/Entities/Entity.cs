using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EntityType
{
    Cork,
    Vial,
    Abhorwretch,
    Shade,
    Wanderer,
    Hallucination
}

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected EntityType EntityType;
    [SerializeField] protected Color Color;
    [Range(0, 20)]
    [SerializeField] protected int AILevel = 10;
    [Range(1, 10)]
    [SerializeField] protected float MovementOpportunityCooldown = 4.9f; 
    [Space]
    [SerializeField] protected Room CurrentRoom;
    protected Route currentRoute;
    protected int routeProgress = 0;

    private void OnEnable()
    {       
        StartCoroutine("_BehaviourLoop");
    }

    private void OnDisable()
    {
        Log("AI disabled in " + CurrentRoom.name + ".");
        StopCoroutine("_BehaviourLoop");
        CurrentRoom.LeaveRoom(EntityType);
    }

    protected virtual bool MovementOpportunity(int max = 21)
    {
        int rnd = UnityEngine.Random.Range(1, max);
        if (AILevel > rnd)
        {
            return true;
        }
        else
        {
            Log("Movement opportunity failed.");
            return false;          
        }
    }

    protected void WalkToNextRoom()
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

            case EntityType.Shade:
                if (currentTime.Hours == 2 || currentTime.Hours == 3 || currentTime.Hours == 4 || currentTime.Hours == 5)
                {
                    AILevel++;
                }
                break;        }
    }

    protected void Log(string message)
    {
        print("<color=#" + ColorUtility.ToHtmlStringRGB(Color) + ">" + EntityType.ToString() + ": </color>" + message);
    }
}