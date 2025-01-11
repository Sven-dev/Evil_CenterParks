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
    public List<Room> ConnectedRooms;
    [SerializeField] private List<Entity> Entities = new List<Entity>();

    [SerializeField] private List<GameObject> CorkVisuals; 

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

    public void EnterRoom(EntityType entity)
    {
        switch (entity)
        {
            case EntityType.Cork:
                CorkVisuals[Random.Range(0, CorkVisuals.Count)].SetActive(true);
                break;
        }

        print(entity.ToString() + " entered " + gameObject.name + ".");
    }

    public void LeaveRoom(EntityType entity)
    {
        switch (entity)
        {
            case EntityType.Cork:
                CorkVisuals[Random.Range(0, CorkVisuals.Count)].SetActive(false);
                break;
        }

        print(entity.ToString() + " left " + gameObject.name + ".");
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

    public bool IsTargetRoom(Room room)
    {
        if (this == room)
        {
            return true;
        }

        return false;
    }
}