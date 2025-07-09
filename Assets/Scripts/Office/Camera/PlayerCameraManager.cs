using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private float Duration;
    [SerializeField] private AnimationCurve MovementCurve;
    [SerializeField] private List<Transform> Pivots;
    [Space]
    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject RightButton;
    [Space]
    [SerializeField] private UnityIntEvent OnCameraPerpectiveChange;

    private int ActivePivot = 1;
    private bool Moving = false;
    private bool RadioAccessible = true;

    public void MoveLeft()
    {
        if (Moving)
        {
            return;
        }

        ActivePivot--;
        if (ActivePivot == 0)
        {
            LeftButton.SetActive(false);
            RightButton.SetActive(true);
        }
        else if (ActivePivot == 1 && RadioAccessible == true)
        {
            RightButton.SetActive(true);
            LeftButton.SetActive(true);
        }
        else
        {
            LeftButton.SetActive(true);
        }

        StartCoroutine(_Move(-1));
        OnCameraPerpectiveChange?.Invoke(ActivePivot);
    }

    public void MoveRight()
    {
        if (Moving)
        {
            return;
        }

        ActivePivot++;
        if (ActivePivot == 2)
        {
            RightButton.SetActive(false);
            LeftButton.SetActive(true);
        }
        else if (ActivePivot == 1 && RadioAccessible == true)
        {
            RightButton.SetActive(true);
            LeftButton.SetActive(true);
        }
        else if (ActivePivot == 1 && !RadioAccessible == true)
        {
            RightButton.SetActive(false);
            LeftButton.SetActive(true);
        }

        StartCoroutine(_Move(1));
        OnCameraPerpectiveChange?.Invoke(ActivePivot);
    }

    private IEnumerator _Move(int direction)
    {
        Moving = true;

        Vector3 startPosition = Pivots[ActivePivot - direction].position;
        Vector3 endPosition = Pivots[ActivePivot].position;

        Quaternion startRotation = Pivots[ActivePivot - direction].rotation;
        Quaternion endRotation = Pivots[ActivePivot].rotation;

        float progress = 0;
        while (progress < 1)
        {
            progress = Mathf.Clamp01(progress += Time.deltaTime / Duration);
            Camera.transform.position = Vector3.Lerp(startPosition, endPosition, MovementCurve.Evaluate(progress));
            Camera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, MovementCurve.Evaluate(progress));
            yield return null;
        }

        Moving = false;
    }

    public void RemoveRadioAccess()
    {
        RadioAccessible = false;
        if (ActivePivot == 1)
        {
            RightButton.SetActive(false);
        }
        else if (ActivePivot == 2)
        {
            MoveLeft();
        }
    }
}