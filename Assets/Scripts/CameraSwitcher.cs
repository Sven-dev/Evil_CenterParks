using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Text CameraIndexLabel;
    [Space]
    [SerializeField] private List<Camera> Cameras;

    private int CameraIndex = 0;

    private void Start()
    {
        Cameras[CameraIndex].enabled = true;
    }

    public void SwitchToCamera(int index)
    {
        Cameras[CameraIndex ].depth = -10;
        CameraIndex = index - 1;
        Cameras[CameraIndex].depth = -9;

        CameraIndexLabel.text = index.ToString("00");
    }
}