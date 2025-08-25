using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuestSystem
{
    public class GuestListItem : MonoBehaviour
    {
        [SerializeField] private Text RoomLabel;

        [SerializeField] private Text ReferenceLabel;
        [SerializeField] private Text NameLabel;
        [SerializeField] private Text ArrivalLabel;
        [SerializeField] private Text DepartureLabel;

        public void SetData(GuestData data)
        {
            RoomLabel.text = data.Room.ToString();
            ReferenceLabel.text = data.Reference.ToString();
            NameLabel.text = data.Name;
            ArrivalLabel.text = data.Arrival.ToString();
            DepartureLabel.text = data.Departure.ToString();
        }
    }
}