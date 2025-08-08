using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guest : MonoBehaviour
{
    [SerializeField] private string Name;
    [SerializeField] private int ReferenceNumber;
    [SerializeField] private int RoomNumber;
    [SerializeField] private int StayDays;

    [SerializeField] private Text NameLabel;
    [SerializeField] private Text ReferenceLabel;
    [SerializeField] private Text RoomLabel;
    [SerializeField] private Text StayLabel;

    public void MoveTo(Transform pivot)
    {
        StartCoroutine(_MoveTo(pivot));
    }

    private IEnumerator _MoveTo(Transform end)
    {
        Vector3 startPosition = transform.position;

        float progress = 0;
        while(progress < 1)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime);
            transform.position = Vector3.Lerp(startPosition, end.position, progress);
            yield return null;
        }
    }
}