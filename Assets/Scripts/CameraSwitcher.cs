using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Perspective Perspective;
    [Space]
    public int ActiveCamera;
    [SerializeField] private float SwitchTime;
    [SerializeField] private List<ParkCamera> Cameras;
    [Space]
    [SerializeField] private CanvasGroup Static;
    [SerializeField] private VideoPlayer VideoPlayer;

    private void Start()
    {
        Cameras[ActiveCamera - 1].EnableCamera();
    }

    public void SwitchToRoom(int index)
    {
        if (Perspective.Active)
        {
            StartCoroutine(_SwitchToRoom(index));
        }
    }

    public IEnumerator _SwitchToRoom(int index)
    {
        Cameras[ActiveCamera - 1].DisableCamera();
        ActiveCamera = index;
        Cameras[ActiveCamera - 1].EnableCamera();

        Static.alpha = 1f;
        VideoPlayer.SetDirectAudioVolume(0, 0.5f);
        yield return new WaitForSeconds(SwitchTime);
        Static.alpha = 0f;
        VideoPlayer.SetDirectAudioVolume(0, 0f);
    }
}
