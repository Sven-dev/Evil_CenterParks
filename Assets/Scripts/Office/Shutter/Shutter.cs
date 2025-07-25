using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shutter : MonoBehaviour
{
    [SerializeField] private Perspective Perspective;
    [Space]
    [SerializeField] private SpacialSlider Slider;
    [SerializeField] private Animator Animator;
    [Space]
    [SerializeField] private OfficeManager OfficeManager;

    private bool ForcedOpen = false;
    public bool ShutterInteractable = false;

    public void Interactable(bool state)
    {
        if (state == true)
        {
            ShutterInteractable = true;
            if (!ForcedOpen)
            {
                Slider.Interactable = true;
            }
        }
        else
        {
            ShutterInteractable = false;
            Slider.Interactable = false;
            StartCoroutine("_FallBackDown");
        }
    }


    public void StopFalling()
    {
        StopCoroutine("_FallBackDown");
    }

    public void StartFalling()
    {
        StartCoroutine("_FallBackDown");
    }

    public void UpdatePosition(float value)
    {
        Animator.SetFloat("Position", value);
        if (value > 0.8f)
        {
            OfficeManager.ShutterOpen = false;
        }
        else
        {
            OfficeManager.ShutterOpen = true;
        }
    }

    /// <summary>
    /// Gets called when Vial is in the room
    /// </summary>
    public void ForceShutterOpen()
    {
        ForcedOpen = true;
        Slider.Interactable = false;
    }

    public void ReleaseShutter()
    {
        ForcedOpen = false;
        Slider.Interactable = ShutterInteractable;      
    }

    private IEnumerator _FallBackDown()
    {
        float shutterPosition = Animator.GetFloat("Position");
        while (shutterPosition > 0)
        {
            shutterPosition = Mathf.Clamp01(shutterPosition - Time.deltaTime);
            Slider.value = shutterPosition;
            yield return null;
        }
    }
}
