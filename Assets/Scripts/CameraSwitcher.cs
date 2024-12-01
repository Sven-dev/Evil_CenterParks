using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private List<Room> Rooms;

    private int CameraIndex = 0;

    private void Start()
    {
        Rooms[CameraIndex].EnableCamera();
    }

    public void SwitchToRoom(int index)
    {
        Rooms[CameraIndex].DisableCamera();
        CameraIndex = index - 1;
        Rooms[CameraIndex].EnableCamera();
    }
}