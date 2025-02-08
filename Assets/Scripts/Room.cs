using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int ID;
    public List<Room> ConnectedRooms;
    [SerializeField] private List<EntityType> Entities = new List<EntityType>();

    [Header("Entity visuals")]
    [SerializeField] private List<GameObject> CorkVisuals;
    [SerializeField] private List<GameObject> VialVisuals;
    [SerializeField] private List<GameObject> WandererVisuals;

    [Space]
    [SerializeField] private UnityVoidEvent OnEntityUpdate;

    [Header("Noise")]
    [Range(0, 5)]
    [SerializeField] private int NoiseLevel = 0;
    [Range(0, 5)]
    [SerializeField] private int NoiseFloor = 0;

    private const int NoiseCeiling = 5;

    [Space]
    [SerializeField] private UnityFloatEvent OnNoiseLevelChange;

    public GuestRoomManager GuestRooms;

    private void Start()
    {
        NoiseLevel = NoiseFloor;
        OnNoiseLevelChange?.Invoke(NoiseLevel);
        StartCoroutine(_ManageNoiseLevel());
    }

    public void EnterRoom(EntityType entity)
    {
        OnEntityUpdate?.Invoke();

        Entities.Add(entity);
        switch (entity)
        {
            case EntityType.Cork:
                CorkVisuals[Random.Range(0, CorkVisuals.Count)].SetActive(true);
                break;
            case EntityType.Vial:
                VialVisuals[Random.Range(0, VialVisuals.Count)].SetActive(true);
                AlterNoiseLevel(+1);
                break;
            case EntityType.Wanderer:
                WandererVisuals[Random.Range(0, WandererVisuals.Count)].SetActive(true);
                break;
        }
    }

    public void LeaveRoom(EntityType entity)
    {
        OnEntityUpdate?.Invoke();

        Entities.Remove(entity);
        switch (entity)
        {
            case EntityType.Cork:
                CorkVisuals[Random.Range(0, CorkVisuals.Count)].SetActive(false);
                break;
            case EntityType.Vial:
                VialVisuals[Random.Range(0, VialVisuals.Count)].SetActive(false);
                if (NoiseLevel != 5)
                {
                    AlterNoiseLevel(-1);
                }
                break;
            case EntityType.Wanderer:
                WandererVisuals[Random.Range(0, WandererVisuals.Count)].SetActive(false);
                break;
        }
    }

    public int GetNoiseLevel()
    {
        return NoiseLevel;
    }

    public void AlterNoiseLevel(int amount)
    {
        NoiseLevel = Mathf.Clamp(NoiseLevel + amount, NoiseFloor, NoiseCeiling);
        OnNoiseLevelChange?.Invoke(NoiseLevel);
    }

    private IEnumerator _ManageNoiseLevel()
    {
        while (true)
        {    
            if (!Entities.Contains(EntityType.Vial))
            {
                yield return new WaitForSecondsRealtime(6);
                NoiseLevel = Mathf.Clamp(NoiseLevel - 2, NoiseFloor, NoiseCeiling);
                OnNoiseLevelChange?.Invoke(NoiseLevel);
            }

            yield return null;
        }
    }

    public bool IsTargetRoom(Room room)
    {
        if (this == room)
        {
            return true;
        }

        return false;
    }
}