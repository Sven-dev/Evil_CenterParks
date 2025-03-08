using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private float Duration;
    [SerializeField] private AnimationCurve MovementCurve;
    private bool Moving = false;

    [SerializeField] private List<Transform> Pivots;
    private int ActivePivot = 1;

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

        StartCoroutine(_Move(-1));
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

        StartCoroutine(_Move(1));       
    }

    private IEnumerator _Move(int direction)
    {
        Moving = true;

        Vector3 startPosition = Pivots[ActivePivot].position;
        Vector3 endPosition = Pivots[ActivePivot + direction].position;

        Quaternion startRotation = Pivots[ActivePivot].rotation;
        Quaternion endRotation = Pivots[ActivePivot + direction].rotation;

        float progress = 0;
        while (progress < 1)
        {
            progress = Mathf.Clamp01(progress += Time.deltaTime / Duration);
            transform.position = Vector3.Lerp(startPosition, endPosition, MovementCurve.Evaluate(progress));
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, MovementCurve.Evaluate(progress));
            yield return null;
        }

        ActivePivot += direction;
        Moving = false;
    }
}
