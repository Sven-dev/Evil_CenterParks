using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampManager : MonoBehaviour
{
    public static LampManager Instance;

    [SerializeField] private List<Lamp> Lamps;

    private void Awake()
    {
        Instance = this;
    }

    public void Flicker()
    {
        foreach (Lamp light in Lamps)
        {
            light.Flicker();
        }
    }
}