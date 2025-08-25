using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuestSystem
{
    public class Guest : MonoBehaviour
    {
        [SerializeField] private Text NameLabel;
        [SerializeField] private Text ReferenceLabel;
        [SerializeField] private Text DepartureLabel;

        public void SetData(GuestData data)
        {
            NameLabel.text += data.Name;
            ReferenceLabel.text += data.Reference.ToString("D4");
            DepartureLabel.text += data.Departure.ToString();
        }

        public void MoveTo(Transform pivot)
        {
            StartCoroutine(_MoveTo(pivot));
        }

        private IEnumerator _MoveTo(Transform end)
        {
            Vector3 startPosition = transform.position;

            float progress = 0;
            while (progress < 1)
            {
                progress = Mathf.Clamp01(progress + Time.deltaTime);
                transform.position = Vector3.Lerp(startPosition, end.position, progress);
                yield return null;
            }
        }
    }
}