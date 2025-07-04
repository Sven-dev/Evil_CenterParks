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
        //Camera.enabled = true;
        Camera.depth = -9;

        StartCoroutine("_Temp");
    }

    public void DisableCamera()
    {
        Camera.depth = -10;
        //Camera.enabled = false;

        StopCoroutine("_Temp");
    }

    private IEnumerator _Temp()
    {
        while (true)
        {
            Camera.Render();
            yield return new WaitForSecondsRealtime(0.1f);
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