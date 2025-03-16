using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private int ActivePerspective;
    [Space]
    [SerializeField] private List<ParkCamera> Cameras;
    [SerializeField] private List<Button> Buttons;

    private int CameraIndex = 0;

    private void Start()
    {
        Cameras[CameraIndex].EnableCamera();
    }

    public void ToggleActive(int cameraPerspective)
    {
        if (cameraPerspective == ActivePerspective)
        {
            foreach(Button button in Buttons)
            {
                button.interactable = true;
            }
        }
        else
        {
            foreach (Button button in Buttons)
            {
                button.interactable = false;
            }
        }
    }

    public void SwitchToRoom(int index)
    {
        Cameras[CameraIndex].DisableCamera();
        CameraIndex = index - 1;
        Cameras[CameraIndex].EnableCamera();
    }
}