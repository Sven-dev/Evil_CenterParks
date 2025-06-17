using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampManager : MonoBehaviour
{
    public static LampManager Instance;

    [SerializeField] private List<Lamp> Lamps;

    public void Flicker()
    {
        foreach (Lamp light in Lamps)
        {
            light.Flicker();
        }
    }
}