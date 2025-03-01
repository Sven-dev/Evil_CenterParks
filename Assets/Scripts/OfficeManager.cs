using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    [SerializeField] private List<Camera> Cameras;

    private int CameraIndex = 0;

    [SerializeField] private UnityVoidEvent OnDeath;

    public bool ShutterOpen = true;

    private void Start()
    {
        Cameras[CameraIndex].enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cameras[CameraIndex].enabled = false;

            CameraIndex++;
            if (CameraIndex >= Cameras.Count)
            {
                CameraIndex = 0;
            }

            Cameras[CameraIndex].enabled = true;
        }

        if (Input.GetKey(KeyCode.V))
        {
            ShutterOpen = false;
        }
        else
        {
            ShutterOpen = true;
        }
    }

    public void Kill()
    {
        OnDeath?.Invoke();
    }

    public void CorkEnter()
    {
        CorkDeathTimer = _CorkDeathTimer();
        StartCoroutine(CorkDeathTimer);
    }

    private IEnumerator CorkDeathTimer;
    private IEnumerator _CorkDeathTimer()
    {
        float timeTillDeath = Random.Range(4f, 6f);
        while (timeTillDeath > 0)
        {
            if (!ShutterOpen)
            {
                float holdDownTime = Random.Range(4f, 6f);
                while (holdDownTime > 0)
                {
                    if (ShutterOpen)
                    {
                        OnDeath?.Invoke();
                        StopCoroutine(CorkDeathTimer);
                    }

                    holdDownTime -= Time.deltaTime;
                    yield return null;
                }

                //To do: Force cork to leave the room
                StopCoroutine(CorkDeathTimer);
            }

            timeTillDeath -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        if (ShutterOpen)
        {
            OnDeath?.Invoke();
            StopCoroutine(CorkDeathTimer);
        }

        //To do: Force cork to leave the room
    }
}