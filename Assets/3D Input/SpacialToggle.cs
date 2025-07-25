using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacialToggle : MonoBehaviour
{
    public bool Interactable = true;
    public bool TurnedOn = false;
    [Space]
    public UnityBoolEvent OnToggle;

    private void OnMouseDown()
    {
        if (Interactable)
        {
            TurnedOn = !TurnedOn;
            OnToggle?.Invoke(TurnedOn);
        }
    }
}