using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private List<Camera> Cameras;

    private int CameraIndex = 0;

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
    }
}