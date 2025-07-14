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
    [Range(1, 10)]
    [SerializeField] private float MovementOpportunityCooldown = 4.9f;
    [Range(0, 3)]
    public int Bloodlust = 0;
    [Space]
    [Range(0, 20)]
    [SerializeField] protected int AILevel = 10;
    [SerializeField] private List<EntityLevelIncreaseWrapper> AILevelIncreases;
    [Space]
    [SerializeField] protected Room CurrentRoom;

    protected float Cooldown
    {
        get { return MovementOpportunityCooldown * (1 - Bloodlust / 10f); }
    }

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
        foreach (EntityLevelIncreaseWrapper data in AILevelIncreases)
        {
            if (currentTime.Hours == data.Time)
            {
                AILevel = Mathf.Clamp(AILevel + data.IncreaseBy, 0, 20);
            }
        }
    }

    protected void Log(string message)
    {
        print("<color=#" + ColorUtility.ToHtmlStringRGB(Color) + ">" + EntityType.ToString() + ": </color>" + message);
    }
}

[Serializable]
public class EntityLevelIncreaseWrapper
{
    [Tooltip("The hour when the increase should happen.")]
    public int Time;
    [Range(-3, 3)]
    public int IncreaseBy = 1;
}