using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int ID;
    [SerializeField] private bool HasHouses = false;
    [Range(0, 5)]
    [SerializeField] private int NoiseLevel = 0;
    [Range(0, 5)]
    [SerializeField] private int NoiseFloor = 0;
    [Space]
    public List<Room> ConnectedRooms;
    [SerializeField] private List<EntityType> NoiseMakers;
    [SerializeField] private List<EntityType> Entities = new List<EntityType>();

    [Header("Entity visuals")]
    [SerializeField] private List<GameObject> CorkVisuals; 
    [SerializeField] private List<GameObject> VialVisuals; 

    private const int NoiseCeiling = 5;

    private void Start()
    {
        NoiseLevel = NoiseFloor;
        StartCoroutine(_ManageNoiseLevel());
    }

    public void EnterRoom(EntityType entity)
    {
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
        }
    }

    public void LeaveRoom(EntityType entity)
    {
        Entities.Remove(entity);
        switch (entity)
        {
            case EntityType.Cork:
                CorkVisuals[Random.Range(0, CorkVisuals.Count)].SetActive(false);
                break;
            case EntityType.Vial:
                VialVisuals[Random.Range(0, VialVisuals.Count)].SetActive(false);
                AlterNoiseLevel(-1);
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
    }

    private IEnumerator _ManageNoiseLevel()
    {
        while (true)
        {    
            if (!Entities.Contains(EntityType.Vial))
            {
                yield return new WaitForSecondsRealtime(6);
                NoiseLevel = Mathf.Clamp(NoiseLevel - 2, NoiseFloor, NoiseCeiling);                
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