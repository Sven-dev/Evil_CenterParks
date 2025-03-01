using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shutter : MonoBehaviour
{
    [SerializeField] private Slider Slider;
    [SerializeField] private Animator Animator;
    [Space]
    [SerializeField] private OfficeManager OfficeManager;

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
