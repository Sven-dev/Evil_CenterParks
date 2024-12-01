using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [Space]
    [SerializeField] private bool HasHouses = false;
    [Range(0, 5)]
    [SerializeField] private int NoiseLevel = 0;
    [Range(0, 5)]
    [SerializeField] private int NoiseFloor = 0;
    [Space]
    [SerializeField] private List<Entity> Entities = new List<Entity>();

    private int NoiseCeiling = 5;
    private bool NoiseEntityInRoom = false;

    private void Start()
    {
        NoiseLevel = NoiseFloor;
        StartCoroutine(_ManageNoiseLevel());
    }

    public void EnableCamera()
    {
        Camera.depth = -9;
    }

    public void DisableCamera()
    {
        Camera.depth = -10;
    }

    public void EnterRoom(Entity entity)
    {
        Entities.Add(entity);

        if (entity.NoiseMaker)
        {
            NoiseEntityInRoom = true;
        }
    }

    public void LeaveRoom(Entity entity)
    {
        Entities.Remove(entity);

        bool noiseEntitiesPresent = true;
        foreach (Entity e in Entities)
        {
            if (e.NoiseMaker)
            {
                noiseEntitiesPresent = false;
            }
        }

        NoiseEntityInRoom = noiseEntitiesPresent;
    }

    public int GetNoiseLevel()
    {
        return NoiseLevel;
    }

    public void RaiseNoiseLevel(int amount)
    {
        NoiseLevel = Mathf.Clamp(NoiseLevel + amount, NoiseFloor, NoiseCeiling);
    }

    private IEnumerator _ManageNoiseLevel()
    {
        while(true)
        {
            while (NoiseEntityInRoom)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }

            NoiseLevel = Mathf.Clamp(NoiseLevel - 2, NoiseFloor, NoiseCeiling);
            yield return new WaitForSecondsRealtime(6);
        }
    }
}