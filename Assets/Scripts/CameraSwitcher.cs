using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private int ActivePerspective;
    [Space]
    [SerializeField] private List<ParkCamera> Cameras;
    [SerializeField] private List<Button> Buttons;
    [Space]
    [SerializeField] private float SwitchTime;
    [SerializeField] private RawImage Static;
    [SerializeField] private VideoPlayer VideoPlayer;

    public int CameraIndex = 0;

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
        StartCoroutine(_SwitchToRoom(index));
    }

    public IEnumerator _SwitchToRoom(int index)
    {
        Cameras[CameraIndex].DisableCamera();
        CameraIndex = index - 1;
        Cameras[CameraIndex].EnableCamera();

        Color c = Static.color;
        c.a = 1;
        Static.color = c;

        VideoPlayer.SetDirectAudioVolume(0, 0.5f);
        yield return new WaitForSeconds(SwitchTime);
        VideoPlayer.SetDirectAudioVolume(0, 0f);

        c.a = 0.1f;
        Static.color = c;
    }
}
