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

    public static float Framerate = 10;

    public void EnableCamera()
    {
        //Camera.enabled = true;
        Camera.depth = -9;

        StartCoroutine("_RenderLoop");
    }

    public void DisableCamera()
    {
        Camera.depth = -10;
        //Camera.enabled = false;

        StopCoroutine("_RenderLoop");
    }

    private IEnumerator _RenderLoop()
    {
        while (true)
        {
            Camera.Render();
            yield return new WaitForSecondsRealtime(1 / Framerate);
        }
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