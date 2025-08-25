using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GuestSystem
{
    public class Guest : MonoBehaviour
    {
        [HideInInspector] public ArrivalData Data;

        [SerializeField] private Text NameLabel;
        [SerializeField] private Text ReferenceLabel;
        [SerializeField] private Text DepartureLabel;

        public void SetData(ArrivalData Guest)
        {
            Data = Guest;

            NameLabel.text += Guest.Info.Name;
            ReferenceLabel.text += Guest.Info.Reference.ToString("D4");
            DepartureLabel.text += Guest.Info.Departure.ToString();
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