using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkCamera : MonoBehaviour
{
    [SerializeField] private Camera Camera;

    public void EnableCamera()
    {
        Camera.depth = -9;
    }

    public void DisableCamera()
    {
        Camera.depth = -10;
    }
}