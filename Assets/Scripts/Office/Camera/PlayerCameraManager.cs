using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private float Duration;
    [SerializeField] private AnimationCurve MovementCurve;
    private bool Moving = false;

    [SerializeField] private List<Transform> Pivots;
    private int ActivePivot = 1;

    [SerializeField] private UnityIntEvent OnCameraPerpectiveChange;

    public void MoveLeft()
    {
        if (Moving)
        {
            return;
        }

        if (ActivePivot == 0)
        {
            return;
        }

        ActivePivot--;
        StartCoroutine(_Move(-1));
        OnCameraPerpectiveChange?.Invoke(ActivePivot);
    }

    public void MoveRight()
    {
        if (Moving)
        {
            return;
        }

        if (ActivePivot == Pivots.Count - 1)
        {
            return;
        }

        ActivePivot++;
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
}
