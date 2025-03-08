using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private float Duration;
    [SerializeField] private AnimationCurve MovementCurve;
    private bool Rotating = false;

    public void RotateLeft()
    {
        if (!Rotating)
        {
            StartCoroutine(_Rotate(-1));
        }
    }

    public void RotateRight()
    {
        if (!Rotating)
        {
            StartCoroutine(_Rotate(1));
        }
    }

    private IEnumerator _Rotate(int direction)
    {
        Rotating = true;

        Quaternion start = transform.rotation;
        Quaternion end = start * Quaternion.Euler(direction * Vector3.up * 90);
        float progress = 0;
        while (progress < 1)
        {
            progress = Mathf.Clamp01(progress += Time.deltaTime / Duration);
            transform.rotation = Quaternion.Lerp(start, end, MovementCurve.Evaluate(progress));
            yield return null;
        }

        Rotating = false;
    }
}
