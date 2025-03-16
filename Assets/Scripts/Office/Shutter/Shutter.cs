using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shutter : MonoBehaviour
{
    [SerializeField] private int ActivePerspective;
    [Space]
    [SerializeField] private Slider Slider;
    [SerializeField] private Animator Animator;
    [Space]
    [SerializeField] private OfficeManager OfficeManager;

    private bool ForcedOpen = false;
    private bool ShutterInteractable = false;

    public void ToggleActive(int cameraPerspective)
    {
        if (cameraPerspective == ActivePerspective)
        {
            ShutterInteractable = true;
            if (!ForcedOpen)
            {
                Slider.interactable = true;
            }
        }
        else
        {
            ShutterInteractable = false;
            Slider.interactable = false;                
        }        
    }

    public void StopFalling()
    {
        StopCoroutine(_FallBackDown());
    }

    public void StartFalling()
    {
        StartCoroutine(_FallBackDown());
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
        Slider.interactable = false;
    }

    public void ReleaseShutter()
    {
        ForcedOpen = false;
        Slider.interactable = ShutterInteractable;      
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
