using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkCamera : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [Space]
    [SerializeField] private float Downtime;
    [SerializeField] private GameObject Static;

    public void EnableCamera()
    {
        Camera.depth = -9;
    }

    public void DisableCamera()
    {
        Camera.depth = -10;
    }

    public void LoseConnection()
    {
        Static.SetActive(true);
        Invoke("DisableStatic", Downtime);
    }

    private void DisableStatic()
    {
        Static.SetActive(false);
    }
}