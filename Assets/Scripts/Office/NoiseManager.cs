using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance;

    [SerializeField] private Room OfficeRoom;

    private void Awake()
    {
        Instance = this;
    }

    public void AlterNoiseFloor(int amount)
    {
        OfficeRoom.AlterNoiseFloor(amount);
    }
}