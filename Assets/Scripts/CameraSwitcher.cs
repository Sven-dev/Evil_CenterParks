using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private List<ParkCamera> Cameras;

    private int CameraIndex = 0;

    private void Start()
    {
        Cameras[CameraIndex].EnableCamera();
    }

    public void SwitchToRoom(int index)
    {
        Cameras[CameraIndex].DisableCamera();
        CameraIndex = index - 1;
        Cameras[CameraIndex].EnableCamera();
    }
}